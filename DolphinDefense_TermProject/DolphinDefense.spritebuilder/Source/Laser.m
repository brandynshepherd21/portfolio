//
//  Laser.m
//  DolphinDefense
//
//  Created by Dustin Peerce on 11/23/14.
//  Copyright (c) 2014 Apportable. All rights reserved.
//

#import "Laser.h"

static const CGFloat MOVE_SPEED = 300.0f;         // Movement speed
static const CGFloat INITIAL_LIFESPAN = 1.15f;   // Starting lifespan

@implementation Laser {
    CGFloat lifeSpan;   // Remaining time for the laser to exist (to prevent memory leaks)
}

// Called when the file is first loaded
- (void) didLoadFromCCB {
    
    // Initialize the laser's lifespan
    lifeSpan = INITIAL_LIFESPAN;
    
    // Set collision type and enable physics sensor
    self.physicsBody.collisionType = @"laser";
    self.physicsBody.sensor = TRUE;
}

// Called every frame
- (void) update:(CCTime)delta {
    // Update the laser position
    self.position = ccp(self.position.x + delta*MOVE_SPEED, self.position.y);
    
    // Decrease the laser's lifespan by how much time has passed
    lifeSpan -= delta;
    if (lifeSpan <= 0) {
        // If lifespan has depleted, remove the laser
        [self removeFromParent];
    }
}

@end
