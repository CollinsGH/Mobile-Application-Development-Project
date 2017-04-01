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
+ Make a multilingual application
+ Use at least one external service
+ Only use Microsoft products
+ Successfully submit the application to the Windows Store

### Planning
For this project I decided to make a camera app. The possible features for this application include automatically captioning an image and displayng the photos on a map, where each photo was taken.

### Development

#### Album
The main page of this application contains an album displaying all the users photos. When a photo in the album is tapped, the user brought to the image page. It also has a menu to navigate to the other pages.

#### Image
The image page displays a full size version of a particular image.

#### Camera
The camera aspect of this project required both the Camera and Microphone capabilities in the manifest file. When the camera page is opened a preview of the camera is shown. At the bottom of the page is a back button, which brings the user back to the main page, and capture button, which takes a picture. When the capture button is clicked both the preview control and capture button fades to inform the user the picture was taken.

#### Map
I choose to use Bing Maps for the map aspect of this project. An application key was required to use Bing Maps. When the map page loads a list of photos is retrieved. The Geotag metadata is then extracted from the image files and are plotted on the map. If a map icon, the respective photo is displayed in the top right of the screen.

### Why will the user open this app for a second time?

### Conclusion