
*** IMPORTANT NOTE!!! ***

	-SpriteBuilder is a REQUIRED program to run the application. 
	 Dolphin Defense is an Xcode project, and all of the
	 programming was done inside Xcode, but SpriteBuilder was used
	 to add more features to the game. It's FREE on the App Store.

	-I was originally going to use SpriteKit to make the game, and I
	 shaped the design document around what SpriteKit would allow me
	 to do. However, Spritekit does not work on iOS6 devices. The problem
	 would have been solved if I could have updated the device to iOS7,
	 but the device that we were given is not capable of updating to iOS7
	 because it is a fourth generation device. So, I had to find a way to
	 stay true to the original design document, while also making the game
	 compatible with iOS6 devices.
	
	-All of this could have been accomplished through Cocos2d, which used to
	 be a type of Xcode project (exactly like Spritekit). Since you approved
	 Spritekit, I thought there would be no problem with me using Cocos2d.
	 However, Cocos2d was recently adopted by SpriteBuilder, and so SpriteBuilder
	 has become the only way to use the latest Cocos2d frameworks.

	-If I didn't use SpriteBuilder, I would of had to change the design
	 document, because I wouldn't have known how to make the game without
	 it. But, I'm hoping it's okay because SpriteBuilder is essentially an
	 extension of Xcode, much like Spritekit is. So, Dolphin Defense is still
	 an Xcode project. :)


HOW TO RUN THE APPLICATION
	
	-The application runs through Xcode, so you'll have to open the
	 Xcode project for Dolphin Defense

	-The Xcode project for Dolphin Defense is INSIDE the folder with the
	 ".spritebuilder" extension.

	-As long as SpriteBuilder is installed, the Xcode project should compile
	 and run.


KNOWN COMPILER ISSUES/WARNINGS

	-When running on the simulator, there are 0 errors.

	-When running on a physical device, there are 3 errors. HOWEVER,
	 these errors do NOT cause the build to fail. After researching the
	 errors, I believe they are caused by the device itself. So, there may
	 not be any errors when it is run on another device, but I haven't
	 verified this. Regardless, these 3 errors have never caused the build
	 to fail.


HOW TO PLAY THE GAME

	-After being presented with the main menu, just tap the "Play"
	 button to transition to the Gameplay scene
	
	-Once the Gameplay scene is loaded, you will see the star of the show:
	 the dolphin. 

	-To move the dolphin along the y-axis, simply tilt the device
	 up and down. This functionality is obviously not available in the
	 simulator. 

	-To fire a laser, simply tap the screen anywhere!


THE OBJECTIVE

	-Sharks will be spawning from the right side of the screen, and they
	 will move towards the left side.

	-The objective is to kill as many sharks as possible. You gain a point
	 for every shark that you kill.

	-If a shark touches the dolphin, you lose. A "Restart" button will appear,
	 and you can reset the scene by tapping the button.