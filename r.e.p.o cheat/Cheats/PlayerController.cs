﻿using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;

namespace r.e.p.o_cheat
{
    static class PlayerController
    {
        public static object playerSpeedInstance;
        public static object reviveInstance;
        public static object enemyDirectorInstance;
        public static object playerControllerInstance;
        public static Type playerControllerType = Type.GetType("PlayerController, Assembly-CSharp");

        private static float desiredDelayMultiplier = 1f;
        private static float desiredRateMultiplier = 1f;

        private static void InitializePlayerController()
        {
            if (playerControllerType == null)
            {
                Hax2.Log1("PlayerController type not found.");
                return;
            }

            playerControllerInstance = GameHelper.FindObjectOfType(playerControllerType);
            if (playerControllerInstance == null)
            {
                Hax2.Log1("PlayerController instance not found in current scene.");
            }
            else
            {
                Hax2.Log1("PlayerController instance updated successfully.");
            }
        }

        public static void GodMode()
        {
            var playerControllerType = Type.GetType("PlayerController, Assembly-CSharp");
            if (playerControllerType != null)
            {
                Hax2.Log1("PlayerController found.");

                var playerControllerInstance = GameHelper.FindObjectOfType(playerControllerType);
                if (playerControllerInstance != null)
                {
                    var playerAvatarScriptField = playerControllerInstance.GetType().GetField("playerAvatarScript", BindingFlags.Public | BindingFlags.Instance);
                    if (playerAvatarScriptField != null)
                    {
                        var playerAvatarScriptInstance = playerAvatarScriptField.GetValue(playerControllerInstance);

                        var playerHealthField = playerAvatarScriptInstance.GetType().GetField("playerHealth", BindingFlags.Public | BindingFlags.Instance);
                        if (playerHealthField != null)
                        {
                            var playerHealthInstance = playerHealthField.GetValue(playerAvatarScriptInstance);

                            var godModeField = playerHealthInstance.GetType().GetField("godMode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            if (godModeField != null)
                            {
                                bool currentGodMode = (bool)godModeField.GetValue(playerHealthInstance);

                                bool newGodModeState = !currentGodMode;
                                godModeField.SetValue(playerHealthInstance, newGodModeState);

                                Hax2.godModeActive = !newGodModeState;

                                Hax2.Log1("God Mode " + (newGodModeState ? "enabled" : "disabled"));
                            }
                            else
                            {
                                Hax2.Log1("godMode field not found in playerHealth.");
                            }
                        }
                        else
                        {
                            Hax2.Log1("playerHealth field not found in playerAvatarScript.");
                        }
                    }
                    else
                    {
                        Hax2.Log1("playerAvatarScript field not found in PlayerController.");
                    }
                }
                else
                {
                    Hax2.Log1("playerControllerInstance not found.");
                }
            }
            else
            {
                Hax2.Log1("PlayerController type not found.");
            }
        }

        public static void RemoveSpeed(float sliderValue)
        {
            var playerInSpeedType = Type.GetType("PlayerController, Assembly-CSharp");
            if (playerInSpeedType != null)
            {
                Hax2.Log1("playerInSpeedType n é null");
                playerSpeedInstance = GameHelper.FindObjectOfType(playerInSpeedType);
                if (playerSpeedInstance != null)
                {
                    Hax2.Log1("playerSpeedInstance n é null");
                }
                else
                {
                    Hax2.Log1("playerSpeedInstance null");
                }
            }
            else
            {
                Hax2.Log1("playerInSpeedType null");
            }
            if (playerSpeedInstance != null)
            {
                Hax2.Log1("playerSpeedInstance n é null");

                var playerControllerType = playerSpeedInstance.GetType();

                var moveSpeedField1 = playerControllerType.GetField("MoveSpeed", BindingFlags.Public | BindingFlags.Instance);

                if (moveSpeedField1 != null)
                {
                    moveSpeedField1.SetValue(playerSpeedInstance, sliderValue);
                    Hax2.Log1("MoveSpeed value set to " + sliderValue);
                }
                else
                {
                    Hax2.Log1("MoveSpeed field not found in PlayerController.");
                }
            }
        }

        public static void MaxStamina()
        {
            var playerControllerType = Type.GetType("PlayerController, Assembly-CSharp");
            if (playerControllerType != null)
            {
                Hax2.Log1("PlayerController found.");

                var playerControllerInstance = GameHelper.FindObjectOfType(playerControllerType);
                if (playerControllerInstance != null)
                {
                    var energyCurrentField = playerControllerInstance.GetType().GetField("EnergyCurrent", BindingFlags.Public | BindingFlags.Instance);
                    if (energyCurrentField != null)
                    {
                        if (Hax2.stamineState)
                        {
                            energyCurrentField.SetValue(playerControllerInstance, 999999);
                        }
                        else if (!Hax2.stamineState)
                        {
                            energyCurrentField.SetValue(playerControllerInstance, 40);
                        }

                        Hax2.Log1("EnergyCurrent set to " + (Hax2.stamineState ? 999999 : 40));
                    }
                    else
                    {
                        Hax2.Log1("EnergyCurrent field not found in playerAvatarScript.");
                    }
                }
                else
                {
                    Hax2.Log1("playerControllerInstance not found.");
                }
            }
            else
            {
                Hax2.Log1("PlayerController type not found.");
            }
        }

        public static void DecreaseStaminaRechargeDelay(float delayMultiplier, float rateMultiplier = 1f)
        {
            InitializePlayerController();
            if (playerControllerInstance == null) return;

            desiredDelayMultiplier = delayMultiplier;
            desiredRateMultiplier = rateMultiplier;

            Hax2.Log1("Attempting to decrease stamina recharge delay.");

            var sprintRechargeTimeField = playerControllerType.GetField("sprintRechargeTime", BindingFlags.NonPublic | BindingFlags.Instance);
            if (sprintRechargeTimeField != null)
            {
                float defaultRechargeTime = 1f;
                float newRechargeTime = defaultRechargeTime * delayMultiplier;
                sprintRechargeTimeField.SetValue(playerControllerInstance, newRechargeTime);
                Hax2.Log1($"sprintRechargeTime set to {newRechargeTime} (multiplier: {delayMultiplier})");
            }
            else
            {
                Hax2.Log1("sprintRechargeTime field not found in PlayerController.");
            }

            var sprintRechargeAmountField = playerControllerType.GetField("sprintRechargeAmount", BindingFlags.NonPublic | BindingFlags.Instance);
            if (sprintRechargeAmountField != null)
            {
                float defaultRechargeAmount = 2f;
                float newRechargeAmount = defaultRechargeAmount * rateMultiplier;
                sprintRechargeAmountField.SetValue(playerControllerInstance, newRechargeAmount);
                Hax2.Log1($"sprintRechargeAmount set to {newRechargeAmount} (multiplier: {rateMultiplier})");
            }
            else
            {
                Hax2.Log1("sprintRechargeAmount field not found in PlayerController.");
            }
        }

        public static void ReapplyStaminaSettings()
        {
            InitializePlayerController();
            if (playerControllerInstance != null)
            {
                DecreaseStaminaRechargeDelay(desiredDelayMultiplier, desiredRateMultiplier);
                Hax2.Log1("Reapplied stamina settings after scene change.");
            }
        }
    }
}