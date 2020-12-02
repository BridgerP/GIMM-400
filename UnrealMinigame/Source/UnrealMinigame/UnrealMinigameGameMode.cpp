// Copyright Epic Games, Inc. All Rights Reserved.

#include "UnrealMinigameGameMode.h"
#include "Blueprint/UserWidget.h"
#include "UnrealMinigameCharacter.h"
#include "UObject/ConstructorHelpers.h"

AUnrealMinigameGameMode::AUnrealMinigameGameMode()
	: Super()
{
	// set default pawn class to our Blueprinted character
	static ConstructorHelpers::FClassFinder<APawn> PlayerPawnClassFinder(TEXT("/Game/FirstPersonCPP/Blueprints/FirstPersonCharacter"));
	DefaultPawnClass = PlayerPawnClassFinder.Class;

	// use our custom HUD class
	//HUDClass = AUnrealMinigameHUD::StaticClass();
}

// Called when the game starts or when spawned
void AUnrealMinigameGameMode::BeginPlay()
{
	Super::BeginPlay();

	// Set Material UI to viewport
	if (HUDWidgetClass != nullptr)
	{
		CurrentWidget = CreateWidget<UUserWidget>(GetWorld(), HUDWidgetClass);
		if (CurrentWidget != nullptr)
		{
			CurrentWidget->AddToViewport();
		}
	}
}
