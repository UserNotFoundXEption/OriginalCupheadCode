using System;

// Token: 0x02000095 RID: 149
public enum Sfx
{
	// Token: 0x04000522 RID: 1314
	None,
	// Token: 0x04000523 RID: 1315
	_ = -1,
	// Token: 0x04000524 RID: 1316
	Player_Dash = 1,
	// Token: 0x04000525 RID: 1317
	Player_Hit,
	// Token: 0x04000526 RID: 1318
	Player_Jump,
	// Token: 0x04000527 RID: 1319
	Player_Grounded,
	// Token: 0x04000528 RID: 1320
	Player_Parry,
	// Token: 0x04000529 RID: 1321
	Player_Game_Over,
	// Token: 0x0400052A RID: 1322
	Player_Revive,
	// Token: 0x0400052B RID: 1323
	Player_Plane_Hit = 30,
	// Token: 0x0400052C RID: 1324
	Player_Plane_Shrink,
	// Token: 0x0400052D RID: 1325
	Player_Map_Walk_One = 50,
	// Token: 0x0400052E RID: 1326
	Player_Map_Walk_Two,
	// Token: 0x0400052F RID: 1327
	Player_Weapon_Peashot = 100,
	// Token: 0x04000530 RID: 1328
	Player_Weapon_Peashot_Miss,
	// Token: 0x04000531 RID: 1329
	Player_Weapon_Peashot_Ex,
	// Token: 0x04000532 RID: 1330
	Player_Weapon_Spread = 200,
	// Token: 0x04000533 RID: 1331
	Player_Weapon_Spread_Miss,
	// Token: 0x04000534 RID: 1332
	Player_Weapon_Spread_Ex,
	// Token: 0x04000535 RID: 1333
	Player_Super_Start = 900,
	// Token: 0x04000536 RID: 1334
	Player_Super_Beam,
	// Token: 0x04000537 RID: 1335
	__,
	// Token: 0x04000538 RID: 1336
	Level_Announcer_Ready = 9000,
	// Token: 0x04000539 RID: 1337
	Level_Announcer_Begin,
	// Token: 0x0400053A RID: 1338
	Level_Boss_Death_Explosion,
	// Token: 0x0400053B RID: 1339
	Level_Announcer_Knockout,
	// Token: 0x0400053C RID: 1340
	___,
	// Token: 0x0400053D RID: 1341
	Levels_Pirate_Laugh = 10000,
	// Token: 0x0400053E RID: 1342
	Levels_Pirate_Whistle,
	// Token: 0x0400053F RID: 1343
	Levels_Pirate_Barrel_Smash,
	// Token: 0x04000540 RID: 1344
	Levels_Pirate_Fish_Splash,
	// Token: 0x04000541 RID: 1345
	Levels_Pirate_Peashot,
	// Token: 0x04000542 RID: 1346
	Levels_Pirate_Shark_Bite,
	// Token: 0x04000543 RID: 1347
	Leves_Pirate_Ship_Cannon,
	// Token: 0x04000544 RID: 1348
	Levels_Pirate_Squid_Splash,
	// Token: 0x04000545 RID: 1349
	Levels_Pirate_Whale_Open,
	// Token: 0x04000546 RID: 1350
	Levels_Pirate_Whale_Uvula_Spit,
	// Token: 0x04000547 RID: 1351
	Levels_Pirate_Whale_Uvula_Beam,
	// Token: 0x04000548 RID: 1352
	____,
	// Token: 0x04000549 RID: 1353
	Levels_Frogs_Short_Clap = 11000,
	// Token: 0x0400054A RID: 1354
	Levels_Frogs_Short_Rage_Shoot,
	// Token: 0x0400054B RID: 1355
	Levels_Frogs_Tall_Firefly,
	// Token: 0x0400054C RID: 1356
	Levels_Frogs_Tall_Fan,
	// Token: 0x0400054D RID: 1357
	Levels_Frogs_Morph,
	// Token: 0x0400054E RID: 1358
	Levels_Frogs_Morph_Land,
	// Token: 0x0400054F RID: 1359
	Levels_Frogs_Morph_Coin,
	// Token: 0x04000550 RID: 1360
	Level_Frogs_Tall_Firefly_Die,
	// Token: 0x04000551 RID: 1361
	Level_Frogs_Short_Roll,
	// Token: 0x04000552 RID: 1362
	Level_Frogs_Morph_Slots_Cycle,
	// Token: 0x04000553 RID: 1363
	Level_Frogs_Morph_Slots_Stop,
	// Token: 0x04000554 RID: 1364
	Level_Frogs_Morph_Bison_Flame,
	// Token: 0x04000555 RID: 1365
	_____,
	// Token: 0x04000556 RID: 1366
	Levels_Airship_Jelly_Hurt = 12000,
	// Token: 0x04000557 RID: 1367
	Levels_Airship_Jelly_Walk,
	// Token: 0x04000558 RID: 1368
	Levels_Airship_Jelly_Hit,
	// Token: 0x04000559 RID: 1369
	______,
	// Token: 0x0400055A RID: 1370
	Levels_Veggies_Ground = 13000,
	// Token: 0x0400055B RID: 1371
	Levels_Veggies_Potato_Spit,
	// Token: 0x0400055C RID: 1372
	Levels_Veggies_Carrot_Hurt,
	// Token: 0x0400055D RID: 1373
	Levels_Veggies_Carrot_Psychic_Start,
	// Token: 0x0400055E RID: 1374
	Levels_Veggies_Carrot_Projectile_Death,
	// Token: 0x0400055F RID: 1375
	_______,
	// Token: 0x04000560 RID: 1376
	Level_Train_BlindSpecter_Intro = 14000,
	// Token: 0x04000561 RID: 1377
	Level_Train_BlindSpecter_Shoot,
	// Token: 0x04000562 RID: 1378
	Level_Train_Skeleton_Slap,
	// Token: 0x04000563 RID: 1379
	Level_Train_Top_Explode,
	// Token: 0x04000564 RID: 1380
	Level_Train_Skeleton_Up,
	// Token: 0x04000565 RID: 1381
	Level_Train_Skeleton_Down,
	// Token: 0x04000566 RID: 1382
	Level_Train_Pumpkin_Die,
	// Token: 0x04000567 RID: 1383
	________,
	// Token: 0x04000568 RID: 1384
	Level_FlyingBird_Feathers = 15000,
	// Token: 0x04000569 RID: 1385
	Level_FlyingBird_Small_Bird_Death,
	// Token: 0x0400056A RID: 1386
	Level_FlyingBird_Intro_A,
	// Token: 0x0400056B RID: 1387
	Level_FlyingBird_Intro_B,
	// Token: 0x0400056C RID: 1388
	Level_FlyingBird_Egg_Explode,
	// Token: 0x0400056D RID: 1389
	Level_FlyingBird_Egg_Spit,
	// Token: 0x0400056E RID: 1390
	Level_FlyingBird_Steamwhistle,
	// Token: 0x0400056F RID: 1391
	Level_FlyingBird_Glove_Laser,
	// Token: 0x04000570 RID: 1392
	Level_FlyingBird_Small_Shoot,
	// Token: 0x04000571 RID: 1393
	_________,
	// Token: 0x04000572 RID: 1394
	Level_Bee_Chain = 16000,
	// Token: 0x04000573 RID: 1395
	Level_Bee_Grunt_Death,
	// Token: 0x04000574 RID: 1396
	Level_Bee_Spit,
	// Token: 0x04000575 RID: 1397
	Level_Bee_Magic_Start,
	// Token: 0x04000576 RID: 1398
	Level_Bee_Magic_Go,
	// Token: 0x04000577 RID: 1399
	Level_Bee_Intro,
	// Token: 0x04000578 RID: 1400
	Level_Bee_Security_Bomb_Throw,
	// Token: 0x04000579 RID: 1401
	Level_Bee_Security_Bomb_Explode,
	// Token: 0x0400057A RID: 1402
	Level_Bee_Intro_Knife,
	// Token: 0x0400057B RID: 1403
	Level_Bee_Intro_Snap,
	// Token: 0x0400057C RID: 1404
	__________,
	// Token: 0x0400057D RID: 1405
	Level_Dragon_Intro = 17000,
	// Token: 0x0400057E RID: 1406
	Level_Dragon_Eye_Shot,
	// Token: 0x0400057F RID: 1407
	Level_Dragon_Meteor_Spit,
	// Token: 0x04000580 RID: 1408
	Level_Dragon_Sucking_Air,
	// Token: 0x04000581 RID: 1409
	___________,
	// Token: 0x04000582 RID: 1410
	UI_PlayerToggle_001,
	// Token: 0x04000583 RID: 1411
	UI_PlayerToggle_002,
	// Token: 0x04000584 RID: 1412
	UI_PlayerToggle_003,
	// Token: 0x04000585 RID: 1413
	UI_PlayerSelect_Confirm,
	// Token: 0x04000586 RID: 1414
	UI_OpticalLoop,
	// Token: 0x04000587 RID: 1415
	UI_OpticalStart_001,
	// Token: 0x04000588 RID: 1416
	UI_OpticalStart_002,
	// Token: 0x04000589 RID: 1417
	___________________________________
}
