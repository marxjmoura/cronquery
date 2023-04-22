#!/bin/bash

cd docs

set -e

rm -rf ./dist
[[ `git worktree list | grep gh-pages` ]] && git worktree remove ./gh-pages --force
[[ `git branch --list gh-pages` ]] && git branch -D gh-pages
rm -rf ./gh-pages

if [[ ! `git ls-remote origin --heads gh-pages` ]]; then
  git checkout --orphan gh-pages
  git rm -rf .
  git commit --allow-empty -m "Initial gh-pages commit"
  git push origin gh-pages
fi

npm install
npm run publish

git worktree add ./gh-pages gh-pages

rm -rf ./gh-pages/.[!.git]*
cp -r ./dist/* ./gh-pages/

cd ./gh-pages

git add .
git commit -m "Publish"
git push -f origin gh-pages
