set -ex

cd $(dirname $0)/../

artifactsFolder="./artifacts"

if [ -d $artifactsFolder ]; then
  rm -R $artifactsFolder
fi

mkdir -p $artifactsFolder

dotnet restore ./Aix.ConfigWrapper.sln
dotnet build ./Aix.ConfigWrapper.sln -c Release

dotnet pack ./src/Aix.ConfigWrapper/Aix.ConfigWrapper.csproj -c Release -o $artifactsFolder
dotnet pack ./src/Aix.ConfigWrapper.DB/Aix.ConfigWrapper.DB.csproj -c Release -o $artifactsFolder
