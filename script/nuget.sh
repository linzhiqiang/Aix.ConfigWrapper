set -ex

cd $(dirname $0)/../

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

mkdir -p $artifactsFolder


dotnet build ./src/Aix.ConfigWrapper/Aix.ConfigWrapper.csproj -c Release
dotnet build ./src/Aix.ConfigWrapper.DB/Aix.ConfigWrapper.DB.csproj -c Release

dotnet pack ./src/Aix.ConfigWrapper/Aix.ConfigWrapper.csproj -c Release -o $artifactsFolder
dotnet pack ./src/Aix.ConfigWrapper.DB/Aix.ConfigWrapper.DB.csproj -c Release -o $artifactsFolder

dotnet nuget push ./$artifactsFolder/Aix.ConfigWrapper*.nupkg -k $PRIVATE_NUGET_KEY -s https://www.nuget.org
