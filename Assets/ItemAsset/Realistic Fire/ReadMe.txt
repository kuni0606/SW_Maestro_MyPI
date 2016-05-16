You will find "Fire" prefab in Assets\Resources\Prefabs.
You will find demo scene called "FireScene" in the Demo folder.
When you run the demo scene you should see a fire surrounded by a few objects.
Fire should appear first, smoke and sparks should emerge after a short delay.

"Fire" system consists of 4 particle systems and a point light.

1)Flames Particles system renders flames (using Texture sheet animation).
2)Smoke Particles system renders smoke (using Texture sheet animation).
3)Spark Particle system renders sparks that flight out and spread in the space above the fire.
4)Base particles add glow effect to the base of the fire.
5)Fire light is animated (blinking effect) using FireController script(c#).

Feel free to fiddle around with the settings of each system.

Thanks for buying this asset! Good luck!