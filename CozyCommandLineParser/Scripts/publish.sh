#!/usr/bin/env bash

cd ../bin/Debug
latest=$(ls -t *.nupkg | head -n1)
version=$(echo $latest | grep -oP '(?<=CozyCommandLineParser.).+(?=.nupkg)')

echo "Latest $latest, version $version"

if [[ -z "$version" ]]; then
    echo "Can not parse version number in '$latest'"
    exit 1
fi

tagName="release/$version"
gitOutput=$(git tag -l "$tagName")

# if such tag does not exist
if [[ -z "$gitOutput" ]]; then
    echo "new version $version was found, set tag and publish release"
    git tag $tagName || exit 1
#    git push --tags || exit 1

    # get repo name from git
    remote=$(git config --get remote.origin.url)
    repo=$(basename $remote .git)

    commit=$(git rev-parse HEAD)

    echo "'$remote' '$repo'"

    # POST a new ref to repo via Github API
    curl -s -X POST https://api.github.com/repos/4dv/$repo/git/refs \
    -H "Authorization: token $GITHUB_TOKEN" \
    -d @- << EOF
    {
      "ref": "refs/tags/$tagName",
      "sha": "$commit"
    }
EOF
    dotnet nuget push $latest -k "$NUGET_TOKEN" -s https://api.nuget.org/v3/index.json
    # there is no nuget cli in github dotnet image and dotnet nuget push doesn't work for github registry
    # looks like it need Name UserName parameter which can not be set through dotnet nuget push
#    dotnet nuget push $latest -k "$GITHUB_TOKEN" -s https://nuget.pkg.github.com/4dv/index.json

#    nuget sources Add -Name "GPR" \
#     -Source "https://nuget.pkg.github.com/OWNER/index.json" \
#     -UserName 4dv -Password "$GITHUB_TOKEN"
#
#    nuget push $latest -Source "GPR"
else
    echo "tag already exist"
fi
