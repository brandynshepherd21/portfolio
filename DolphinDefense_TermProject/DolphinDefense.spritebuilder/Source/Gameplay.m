//
//  Gameplay.m
//  DolphinDefense
//
//  Created by Dustin Peerce on 11/23/14.
//  Copyright (c) 2014 Apportable. All rights reserved.
//

#import "Gameplay.h"
#import "Shark.h"
#import <CoreMotion/CoreMotion.h>

static const CGFloat SCROLL_SPEED = 70.0f;      // The scrolling speed of the background/rock objects
static const CGFloat DELAY_TIME = 2.0f;         // Time delay before the "Game Over" UI elements activate

@implementation Gameplay {
    // Dolphin object
    CCSprite *_dolphin;
    
    // Background/rock objects
    CCNode *_backgroundWater1;
    CCNode *_backgroundWater2;
    CCNode *_topRock1;
    CCNode *_topRock2;
    CCNode *_botRock1;
    CCNode *_botRock2;
    
    // Gameplay physics node
    CCPhysicsNode *_physicsNode;
    
    // Restart Button
    CCButton *_restartButton;
    
    // Label for the player's score
    CCLabelTTF *_scoreLabel;
    
    // Label for the "Game Over" text
    CCLabelTTF *_gameOverLabel;
    
    CCNodeGradient *_gameOverBackground;
    
    // The player's score
    NSInteger _points;
    
    // Audio object
    OALSimpleAudio *_audio;
    
    // Motion Manager object
    CMMotionManager *_motionManager;
    
    // Array for storing all of the scrolling objects
    NSArray *_scrollingObjects;
    
    // How much time has passed since the last fired laser
    CGFloat _timeSinceLastLaser;
    
    // How much time until the next shark is spawned
    CGFloat _sharkSpawnTimer;
    
    // Move Speed values for the sharks
    CGFloat _sharkSpeedMin;
    CGFloat _sharkSpeedMax;
    
    // The time delay that occurs before the "Game Over" screen
    id _delayAction;
    
    // The action that calls the endGame method
    id _endGameAction;
    
    // The action sequence that combines the delayAction and endGameAction
    id _gameOverSequence;
    
    // Boolean that remains false until the player loses
    BOOL _isGameOver;
}

// Called when the file is first loaded
- (void) didLoadFromCCB {
    
    // Initialize all variables and objects
    _scrollingObjects = @[_backgroundWater1, _backgroundWater2, _topRock1, _topRock2, _botRock1, _botRock2];
    _isGameOver = FALSE;
    _timeSinceLastLaser = 0;
    _motionManager = [[CMMotionManager alloc] init];
    _audio = [OALSimpleAudio sharedInstance];
    _delayAction = [CCActionDelay actionWithDuration: DELAY_TIME];
    _endGameAction = [CCActionCallFunc actionWithTarget: self selector: @selector(endGame)];
    _gameOverSequence = [CCActionSequence actions: _delayAction, _endGameAction, nil];
    _sharkSpeedMin = 100.0f;
    _sharkSpeedMax = 100.0f;
    
    // Preload the sound effects (to reduce performance drops)
    [_audio preloadEffect:@"sharkExplosion_sfx.wav"];
    [_audio preloadEffect:@"laser_sfx.wav"];
    
    // Play background music (Not Tested Yet)
   // [_audio playBg:@"bgMusic.mp3" loop:YES];
    
    // Set collision delegate
    _physicsNode.collisionDelegate = self;
    
    // Set collision type and enable physics sensor for the dolphin object
    _dolphin.physicsBody.collisionType = @"dolphin";
    _dolphin.physicsBody.sensor = TRUE;
    
    // Enable user interaction
    self.userInteractionEnabled = TRUE;
    
    // Start receiving accelerometer updates
    _motionManager.accelerometerUpdateInterval = 0.25;
    [_motionManager startAccelerometerUpdates];
    
    // Spawn the initial shark object
    [self spawnShark];
}

// Called when the user touches the screen
- (void) touchBegan:(CCTouch *)touch withEvent:(CCTouchEvent *)event {
    
    /*
     * If the game hasn't ended yet, and at least
     * 0.5*delta has passed since the last
     * fired laser, fire another laser
     */
    if (!_isGameOver) {
        if (_timeSinceLastLaser > 0.20f) {
            [self fireLaser];
        }
    }
}

// Called every frame
- (void) update:(CCTime)delta {
    // Don't do anything if the game has ended
    if (!_isGameOver) {
        
        // Update how much time has passed since the last fired laser
        _timeSinceLastLaser += delta;
        
        /*
         * For each object in the array:
         *      1. Update the position (Creates the scrolling effect)
         *      2. Retrieve the world position of the object
         *      3. Convert the world position to a screen position
         *      4. If the object has left the screen, move it to the right (Creates the looping effect)
         */
        for (CCNode *object in _scrollingObjects) {
            // 1.
            object.position = ccp(object.position.x - (delta*SCROLL_SPEED), object.position.y);
            
            // 2.
            CGPoint objectWorldPosition = [object.parent convertToWorldSpace:object.position];
            
            // 3.
            CGPoint objectScreenPosition = [self convertToNodeSpace:objectWorldPosition];
            
            // 4.
            if (objectScreenPosition.x <= (-1 * object.contentSize.width)) {
                object.position = ccp(object.position.x + 2 * object.contentSize.width, object.position.y);
            }
        }
       
        // Retrieve accelerometer data
        CMAccelerometerData *accelerometerData = _motionManager.accelerometerData;
        CMAcceleration acceleration = accelerometerData.acceleration;
        
        // Convert accelerometer data to a new y-coordinate, and clamp the value
        CGFloat newYPosition = _dolphin.position.y + acceleration.x * -1000 * delta;
        newYPosition = clampf(newYPosition, 0.14*self.contentSizeInPoints.height, 0.75*self.contentSizeInPoints.height);
        
        // Update the dolphin position using the new y-coordinate
        _dolphin.position = CGPointMake(_dolphin.position.x, newYPosition);
        
        // Decrease the spawn timer by how much time has passed
        _sharkSpawnTimer -= delta;
        if (_sharkSpawnTimer <= 0) {
            // If spawnTimer has depleted, spawn the next shark object
            [self spawnShark];
        }
    }
}

// Called when a laser needs to be fired
- (void) fireLaser {
    
    // Load a new laser object
    CCNode *laser = [CCBReader load: @"Laser"];

    // Set the position of the laser relative to the dolphin object
    laser.position = ccp((_dolphin.position.x) + 35,
                         (_dolphin.position.y) + 17);
    
    // Play the sound effect for firing a laser
    [_audio playEffect:@"laser_sfx.wav"];
    
    // Add the laser object to the physics node
    [_physicsNode addChild:laser];
    
    // Reset timeSinceLastLaser, since we just fired a laser
    _timeSinceLastLaser = 0;
}

// Called when a shark needs to be spawned
- (void) spawnShark {
    // Load a new shark object
    Shark *shark = (Shark *)[CCBReader load: @"Shark"];
    
    // Update the shark's speed
    [shark updateSpeed: (arc4random_uniform(_sharkSpeedMax - _sharkSpeedMin) + _sharkSpeedMin )];
    
    // Set the position of the shark (with a semi-random y-coordinate)
    shark.position = ccp( 630, arc4random_uniform(165) + 65 );
    
    // Add the shark object to the physics node
    [_physicsNode addChild:shark];
    
    // Set the time for when the next shark will spawn
    if (_points > 100) {
        _sharkSpawnTimer = arc4random_uniform(0.5f) + 0.5f;
    }
    if (_points > 75) {
        _sharkSpawnTimer = arc4random_uniform(0.5f) + 0.75f;
    }
    if (_points > 50) {
        _sharkSpawnTimer = arc4random_uniform(0.5f) + 1.0f;
    }
    else if (_points > 25) {
        _sharkSpawnTimer = arc4random_uniform(0.25f) + 1.25f;
    }
    else {
        _sharkSpawnTimer = arc4random_uniform(0.75f) + 1.25f;
    }
}

// Called when a laser collides with a shark
- (BOOL) ccPhysicsCollisionBegin:(CCPhysicsCollisionPair *)pair laser:(CCNode *)laser shark:(CCNode *)shark {
    
    // Play the sound effect for killing a shark
    [_audio playEffect:@"sharkExplosion_sfx.wav"];
    
    // Load Shark Explosion Particle Effect
    CCParticleSystem *sharkExplosion = (CCParticleSystem *)[CCBReader load:@"SharkExplosion"];
    sharkExplosion.autoRemoveOnFinish = TRUE;
    sharkExplosion.position = shark.position;
    
    // Add the particle effect to the physics node
    [_physicsNode addChild:sharkExplosion];
    
    // Update the player's score
    _points++;
    _scoreLabel.string = [NSString stringWithFormat:@"%d", _points];
    
    // Update shark min speed
    if (_points % 10 == 0) {
        _sharkSpeedMin += 20.0f;
    }
    // Update shark max speed
    _sharkSpeedMax = _sharkSpeedMin + (_points*1.5f);
    
    // Remove the collision objects
    [laser removeFromParent];
    [shark removeFromParent];
    return TRUE;
}


// Called when a dolphin collides with a shark
- (BOOL) ccPhysicsCollisionBegin:(CCPhysicsCollisionPair *)pair dolphin:(CCNode *)dolphin shark:(CCNode *)shark {
    
    // Set the "Game Over" flag
    _isGameOver = TRUE;
    
    // Play the sound effect for getting hit by a shark
    [_audio playEffect:@"sharkExplosion_sfx.wav"];
    
    // Load Dolphin Explosion Particle Effect
    CCParticleSystem *dolphinExplosion = (CCParticleSystem *)[CCBReader load:@"DolphinExplosion"];
    dolphinExplosion.autoRemoveOnFinish = TRUE;
    dolphinExplosion.position = ccp(_dolphin.position.x,
                            _dolphin.position.y);
    
    // Add the particle effect to the physics node
    [_physicsNode addChild:dolphinExplosion];
    
    // Remove the dolphin object
    [_dolphin removeFromParent];
    
    [self stopAllActions];
    // Run the sequence of actions
    [self runAction: _gameOverSequence];
    
    return TRUE;
}

- (void) endGame {
    
    // Pause the scene
    [[CCDirector sharedDirector] pause];
    
    // Enable the "Game Over" UI elements
    _gameOverBackground.visible = TRUE;
    _gameOverLabel.visible = TRUE;
    _restartButton.visible = TRUE;
}

// Called when the "Restart" button is pressed
- (void) restart {
    
    // Reset the "Game Over" flag
    _isGameOver = FALSE;
    
    // Resume the scene
    [[CCDirector sharedDirector] resume];
    
    // Load and run the "Gameplay" scene
    CCScene *scene = [CCBReader loadAsScene:@"Gameplay"];
    [[CCDirector sharedDirector] replaceScene:scene];
}

@end
