# Mobile-App-Development
This project is for my 3rd year Mobile Application Development module in college.

### Brief
Create a Universal Windows Project (UWP) that will each demonstrate the use of Isolated Storage and at least one other sensor or service available on the devices. These include:

+ Accelerometer or Gyroscope
+ GPS (Location Based Services)
+ Sound
+ Network Services (connecting to server for data updates etc)
+ Camera
+ Multi Touch Gesture Management

The UWP application should be well designed with a clear purpose and an answer to the question "why will the user open this app for a second time?". The application must also be submitted for certification on the Windows Store. However, the application does not need to pass certification in order to be considered for grading.

### Goals
The following is a list of goals I set myself for this project.

+ Fullfil the brief outlined above
+ Choose an application that will use a lot of the concepts covered in class
+ Make a multilingual application
+ Develop an application that works on both PC and mobile devices
+ Successfully submit the application to the Windows Store

### Planning
For this project I decided to make a camera app. The possible features for this application include:

+ Take a picture
+ Save the picture
+ Display all the photo in a traditional album
+ Share pictures
+ Store GPS data in the image
+ Displayng the photos on a map
+ Automatically captioning an image

This application will be mainly aimed towards people who like to travel.

### Development
This section of the document will be broken into four sections representing each page in the application.

#### Album
The main page of this application contains an album displaying all the users photos. This album is resized when the size of the window changes. When a photo in the album is clicked, or tapped, the user is brought to the image page. The main page also has a menu to navigate to the other pages.

#### Image
The image page displays a full size version of the image that was clicked in the album. It has a back button which brings the user back to the main page and a share option that allows the user to share the image using other applications installed on the device.

#### Camera
The camera aspect of this project required both the Camera and Microphone capabilities in the manifest file. When the camera page is opened a preview of the camera is shown. At the bottom of the page is a back button, which brings the user back to the main page, a capture button, which takes a picture and a settings button. The settings button, when pressed, will open a side panel from the left of the screen. The panel contains a list of settings including a timer and a location toggle option. These settings are stored in a JSON file using isolated storage, as required in the brief. When the capture button is clicked both the preview control and capture button fades to inform the user the picture was taken.

#### Map
I choose to use Bing Maps for the map aspect of this project. An application key was required to use Bing Maps. When the map page loads a list of photos is retrieved. The Geotag metadata is then extracted from the image files and are plotted on the map. If a map icon is clicked, the respective photo is displayed in the top right of the screen. Again, a back button is provided to bring the user back to the main page.

### Design
A common design theme was followed throughout the application, where the options are positioned at the bottom of screen, overlapping the main content on the page. The buttons are coloured white with a black outline so they are visible despite the background colour. I found this design worked well on both PC and mobile devices. It was important that the icons that were choosen was universally understood since this is a multilingual application.

### Testing
This application was tested on a PC running Windows 10 and a Windows 10 mobile emulator. I didn't have a physical Windows 10 mobile device to test it on. The application was tested with the [Windows App Cert Kit](https://developer.microsoft.com/en-us/windows/develop/app-certification-kit) before being submitted to the Microsoft Store. The application had to be deployed before being tested using the Windows 10 mobile emulator and using the Windows App Cert Kit. The security certificate also had to be installed. The application was also tested in all the supported languages.

### Why will the user open this app for a second time?
As mentioned in the planning section of this document, this application is aimed towards people who travel alot. This application represents photos in two ways. The first is in the common album format and the second is in a location orientated format.

### Conclusion
In my opinion this has been a successful project because it has fullfilled the original brief as it uses isolated storage, the camera and the GPS. I also achieved the goals I outlined for myself. After my first project idea didn't work out I was under time pressure to develop this application. Unfortunately, because of this I didn't have enough time to add the auto caption feature.

Although I was happy to develop this application as it uses a lot of different sensors, if I was doing this project again I would try to pick a more interesting idea.