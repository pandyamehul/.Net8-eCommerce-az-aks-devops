# Configuring Linux in Oracle VirtualBox

Followed below steps to set up a Linux virtual machine in Oracle VirtualBox on Windows 11 host system.

## Prerequisites

- Oracle VirtualBox installed on your Windows 11 system. You can download it from the [official website](https://www.virtualbox.org/).
- A Linux ISO file (e.g., Ubuntu, Fedora, Debian) downloaded from the respective distribution's website. [official Ubuntu download page](https://ubuntu.com/download).
- Sufficient disk space and RAM on your host machine to allocate to the virtual machine.
- Basic knowledge of using VirtualBox and Linux operating systems.
- Administrative privileges on your Windows 11 system to install and configure VirtualBox.
- An active internet connection for downloading updates and additional packages during Linux installation.
- Guest Additions ISO file (usually included with VirtualBox installation) for enhanced functionality.

## Steps to Configure Linux in VirtualBox

1. **Open VirtualBox**: Launch Oracle VirtualBox from your Windows 11 start menu.
2. **Create a New Virtual Machine**:
   - Click on the "New" button.
   - Enter a name for your virtual machine (e.g., "Ubuntu VM").
   - Select the type as "Linux" and choose the appropriate version (e.g., "Ubuntu (64-bit)").
   - Click "Next".
   - Allocate memory (RAM) for the VM. A minimum of 2048 MB is recommended for most Linux distributions.
   - Click "Next".
   - Create a virtual hard disk. Choose "Create a virtual hard disk now" and click "Create".
   - Select the hard disk file type (VDI is recommended) and click "Next".
   - Choose "Dynamically allocated" and click "Next".
   - Set the size of the virtual hard disk (at least 20 GB is recommended) and click "Create".
3. **Configure the Virtual Machine**:
   - Select your newly created VM and click on "Settings".
   - Go to the "Storage" section.
   - Under "Controller: IDE", click on the empty disk icon.
   - Click on the disk icon next to "Optical Drive" and select "Choose a disk file".
   - Browse and select the Linux ISO file you downloaded earlier.
   - Click "OK" to save the settings.  
4. **Start the Virtual Machine**
5. - Select your VM and click "Start".
   - The VM will boot from the Linux ISO file. Follow the on-screen instructions to install Linux on the virtual hard disk.
   - During installation, you may need to configure settings such as language, keyboard layout, timezone, and user account details.
   - Once the installation is complete, restart the VM when prompted.
6. **Install Guest Additions**:
    - After the Linux installation is complete and you are logged in, go to the "Devices" menu in the VirtualBox window.
    - Select "Insert Guest Additions CD image".
    - Open a terminal in your Linux VM and run the Guest Additions installer. You may need to mount the CD drive and execute the installation script (usually `VBoxLinuxAdditions.run`).
    - Download if Guest Additions is not found in the Devices menu from [official VirtualBox website](https://download.virtualbox.org/virtualbox).
    - Follow the prompts to complete the installation and then restart your VM.
    - Guest Additions will provide better integration, including improved graphics support, shared clipboard, and shared folders between the host and guest systems.
7. **Post-Installation Configuration**:
    - Update your Linux system by running the package manager (e.g., `sudo apt update && sudo apt upgrade` for Ubuntu).
    - Install any additional software or tools you need for your work.
    - Configure network settings if necessary to ensure internet connectivity within the VM.
    - Set up shared folders if you want to share files between your host and guest systems.
    - Adjust display settings for better resolution and performance.

## Troubleshooting

- Following faced and below commands used to enable share drive and also clipboard between host and guest.
  
```bash
sudo usermod -aG vboxsf $USER
sudo adduser $USER vboxsf

# check user available in vboxsf group
lsmod | grep vboxsf

# navigate to Guest Additions CD mount point and run the installer
cd /media/$USER/VBox_GAs_*/ 
sudo ./VBoxLinuxAdditions.run

# bzip2not found
sudo apt update && sudo apt install -y bzip2
sudo apt update
sudo apt install build-essential dkms linux-headers-$(uname -r)

```

- Make sure on windows host, folder is enabaled with read/write permission for share drive to work.
