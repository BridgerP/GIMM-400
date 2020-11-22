// Copyright Epic Games, Inc. All Rights Reserved.

#pragma once 

#include "CoreMinimal.h"
#include "GameFramework/HUD.h"
#include "UnrealMinigameHUD.generated.h"

UCLASS()
class AUnrealMinigameHUD : public AHUD
{
	GENERATED_BODY()

public:
	AUnrealMinigameHUD();

	/** Primary draw call for the HUD */
	virtual void DrawHUD() override;

private:
	/** Crosshair asset pointer */
	class UTexture2D* CrosshairTex;

};

