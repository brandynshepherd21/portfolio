//
//  MainScene.m
//  DolphinDefense
//
//  Created by Dustin Peerce on 11/23/14.
//  Copyright (c) 2014 Apportable. All rights reserved.
//

#import "MainScene.h"


@implementation MainScene

// Called when the "Play" button is pressed
- (void) play {
    // Load and run the "Gameplay" scene
    CCScene *gameplayScene = [CCBReader loadAsScene:@"Gameplay"];
    [[CCDirector sharedDirector] replaceScene:gameplayScene];
}

@end
