// Copyright Epic Games, Inc. All Rights Reserved.

#include "UnrealMinigameGameMode.h"
#include "UnrealMinigameHUD.h"
#include "UnrealMinigameCharacter.h"
#include "UObject/ConstructorHelpers.h"

AUnrealMinigameGameMode::AUnrealMinigameGameMode()
	: Super()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPersonCPP/Blueprints/FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

	// use our custom HUD class
	HUDClass = AUnrealMinigameHUD::StaticClass();
}
