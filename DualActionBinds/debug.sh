pkill -f OpenTabletDriver.Daemon
pkill -f OpenTabletDriver.UX.Gtk

mkdir -p ~/.config/OpenTabletDriver/Plugins/DualActionBinds/
cp bin/Debug/net6.0/DualActionBinds.dll ~/.config/OpenTabletDriver/Plugins/DualActionBinds/DualActionBinds.dll

otd-gui > /dev/null 2>&1 & otd-daemon
