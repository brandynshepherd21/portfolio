//
//  Shark.m
//  DolphinDefense
//
//  Created by Dustin Peerce on 11/24/14.
//  Copyright (c) 2014 Apportable. All rights reserved.
//

#import "Shark.h"

static const CGFloat INITIAL_LIFESPAN = 7.5f;    // Starting lifespan

@implementation Shark {
    CGFloat lifeSpan;   // Remaining time for the shark to exist (to prevent memory leaks)
    CGFloat moveSpeed;   // Movement Speed
}

// Called when the file is first loaded
- (void) didLoadFromCCB {
    
    // Initialize the shark's lifespan and speed
    lifeSpan = INITIAL_LIFESPAN;
    moveSpeed = 100.0f;
    
    // Set collision type and enable physics sensor
    self.physicsBody.collisionType = @"shark";
    self.physicsBody.sensor = TRUE;
}

// Called every frame
- (void) update:(CCTime)delta {
    // Update the shark position
    self.position = ccp(self.position.x - delta*moveSpeed, self.position.y);
    
    // Decrease the shark's lifespan by how much time has passed
    lifeSpan -= delta;
    if (lifeSpan <= 0) {
        // If lifespan has depleted, remove the shark
        [self removeFromParent];
    }
}

- (void) updateSpeed: (CGFloat)speed {
    moveSpeed = speed;
}

@end