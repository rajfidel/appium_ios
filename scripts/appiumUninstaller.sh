#!/bin/bash
npm ls -gp --depth=0 | awk -F/ '/node_modules/ && !/\/npm$/ {print $NF}' | xargs npm -g rm
npm uninstall npm -g
brew remove --force $(brew list) --ignore-dependencies
echo "y" | ruby -e "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/uninstall)"
