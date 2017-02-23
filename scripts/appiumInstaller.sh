#!/bin/sh
echo "Perform the following steps of they have not been performed already:"
echo "1. Install Xcode along with any additional components if prompted, login into Xcode using your Developer account"
echo "2. Download developer certificate from Apple Developer and click to install into Keychain login"
echo "3. Run 'xcode-select --install' from terminal to completion"
read -n 1 -s -p "Press any key to continue"

echo "****USER PROMPT****Reconnect iPhone to Mac and set sleep setting to Never. Remove existing WebDriver application from iPhone"
read -n 1 -s -p "Press any key to continue"
echo -ne '\n' | ruby -e "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install)"
brew update
brew install node
npm install -g npm
npm install -g appium #'npm install -g appium --no-shrinkwrap' to get latest updates
npm install -g appium-doctor
npm install wd
cd /usr/local/lib/node_modules/appium/node_modules/appium-xcuitest-driver/WebDriverAgent
brew install carthage
npm i -g webpack
./Scripts/bootstrap.sh -d
brew install --HEAD libimobiledevice
brew install ideviceinstaller
brew install ios-deploy
gem install xcpretty --user-install
npm install -g deviceconsole
./Scripts/build.sh
echo "****USER PROMPT****Open /usr/local/lib/node_modules/appium/node_modules/appium-xcuitest-driver/WebDriverAgent/WebDriverAgent.xcodeproj in Xcode, select your development team for Target 'WebDriverAgentLib' and 'WebDriverAgentRunner' (This should auto select Signing Certificate)"
read -n 1 -s -p "Press any key to continue"
UDID=$(system_profiler SPUSBDataType | awk '/(iPhone|iPad)/ { s = 1 }; s == 1 && /Serial Number:/ { print $3; s = 0 }')
xcodebuild -project WebDriverAgent.xcodeproj -scheme WebDriverAgentRunner -destination id=$UDID test
