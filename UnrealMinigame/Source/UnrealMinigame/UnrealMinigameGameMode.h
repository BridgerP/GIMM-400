// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/GameModeBase.h"
#include "UnrealMinigameGameMode.generated.h"

UCLASS(minimalapi)
class AUnrealMinigameGameMode : public AGameModeBase
{
	GENERATED_BODY()

public:
	AUnrealMinigameGameMode();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

protected:
	/** Widget class to use for HUD screen*/
	UPROPERTY(EditDefaultsOnly, BlueprintReadWrite, Category = "Widget", meta = (BlueprintProtected = true))
	TSubclassOf<class UUserWidget> HUDWidgetClass;

	/** The instance of the HUD Widget class*/
	UPROPERTY(BlueprintReadOnly, Category = "Widget")
	class UUserWidget* CurrentWidget;
};



