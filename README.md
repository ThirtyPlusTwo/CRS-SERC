# CRS-F1 - Cratera's Racing System (for Space Engineers)

### Version 13.1.0
- Pit Limiter
- Drag Reduction System (DRS)
- Energy Recovery System (ERS)
- Onboard Race Information
- Tyre Degradation
- Race Flags Effects (Yellow, Red)
- Weather Effects
- Drafting System
- Race Positions Information
- Auto HudLcd Plugin Setup
- Rear Mirror Sensors

## How to Setup
1. Place a Programmable Block on your car;
2. Place a Light Panel or a Transparent LCD at the back of your car, then create a group with it called "Brakelight";
3. Place a Sensor below the tip of your car (facing down, the top of the sensor should be pointing forward the car), then name it "Drafting Sensor";
4. Optionally, if your programmable block is not visible from the cockpit, you can place an LCD to show onboard information, name it "Driver LCD";
5. You can, also optionally, place two sensors at the back of the rear suspensions naming accordingly "Mirror Sensor Right" and "Mirror Sensor Left";
6. Open the Control Panel, look for your Programmable Block, click on the "Edit" button, then copy and paste the script;
7. Make sure to set the following variables according to your needs:
   - TEAM_TAG => 3 Letters that represent your team, if you're not in a team it's going to be "XXX"
   - DRIVER_NAME => Replace "Guest" with "Your Name"
   - DRIVER_NUMBER => Set a number of your preference from 0 to 99, make sure no other driver uses it.
   - DEFAULT_SUSPENSION_STRENGTH => Set the strength percentage you use on your car suspensions, remember to put an "f" at the end of the number, e.g.: 6.32f;
8. If you want to see onboard race information (Current Lap, Position, Lap Time, Tyre Wear), you must have an Antenna on your car. 
9. Once you have set the values, click on the "Check Code" button. A success message should pop-up (if not, repeat the previous steps);
10. After closing the pop-up, click on "OK" in the Editor;
11. Set up the arguments to the Programmable Block on your car's hotbar, so you can execute commands for your cockpit, then you're ready to race.

**Note: After setting this up, your grid name will change to the standardized name "TEAM_TAG #DRIVER_NUMBER-DRIVER_NAME", e.g.: "CPS #21-Cratera".**

## Arguments
Here's a list of all arguments supported by the current script:
- LMT     => Toggle Pit Limiter
- LMT_ON  => Activate Pit Limiter
- LMT_OFF => Deactivate Pit Limiter
- DRS     => Toggle Drag Reduction System (DRS)
- DRS_ON  => Activate DRS
- DRS_OFF => Deactivate DRS
- ERS     => Toggle Energy Recovery System (ERS)
- ERS_ON  => Activate ERS
- ERS_OFF => Deactivate ERS
- ULTRA   => Switch to Ultra Tyres
- SOFT    => Switch to Soft Tyres
- MEDIUM  => Switch to Medium Tyres
- HARD    => Switch to Hard Tyres
- EXTRA   => Switch to Extra Tyres
- INT     => Switch to Intermediate Tyres
- WET     => Switch to Wet Tyres
- FLIP    => Flips the car if it's upside-down (You need to manually set up gyro override)

## Tyre Degradation
Wheels are going to have less friction over time, affecting the overall performance of your car. You'll see the Tyre Wear % on your onboard screen, once it reaches 0% a random wheel will pop off your car as a puncture.

In order to change wheels, you have to go to the pits with the Pit Limiter active and fully stop your car. Then you will be able to switch tyres using the arguments: ULTRA, SOFT, MEDIUM, HARD, EXTRA, INT, WET. Notice that once you change tyres, the Tyre Wear % changes to 100%, and a letter going to be displayed reprsenting the selected compound (U, S, M, H, X, I, W) and your Brakelight will change color, making your current compound visible to others. The image below show the specs for each compound type:

![alt text](https://i.imgur.com/AWQ4lpc.png)

**Note: For now, the drive style won't matter too much on the tyre degradation rate, it will be pretty similar for everyone. The degradation rate is only based on current speed (even if the wheels are not touching the ground): the faster you go, the faster your tyre will degrade. (At 90m/s it reaches the maximum rate)**

## FAQ
- **How does the DRS work?**
  - It sets the strength of all your suspensions to 100% while active, which allows your car to reach 100m/s. But be careful, it might only be good to use on long straights, and bumpy surfaces might put your car in the air while DRS is active.
  
- **How does the ERS work?**
  - It sets the power of all your suspensions to 100%, overclocks it and increase the top speed while active, consuming the ERS charge when you push the throttle. When deactivated, it will recharge the ERS while moving.
  
- **How does the Drafting System work?**
  - When you're behind another car (detected by the Drafting Sensor) and it is above 70m/s (and you're above 50m/s), your car starts to draft: your suspensions gets 100% wheel power and your speed limit is set to unlimited. It remains active for a little while after moving for the overtake.
  
- **Do I need to setup CRS-F1 script on my car to be detected by the race control script?"**
  - No, once you cross the start line you are being tracked. But in order to see your onboard race data, use commands and have the tyre degradation effect on your car, you need to set up this script on your car.
  
- **Is there any way to stop tyre degradation?**
  - No, unless you turn off your Programable Block (which would be cheating during a race). Make sure to change your tyres before it's too late.

- **Why these values for the tyres?**
  - The lifespans are based on the Fibonacci sequence, notice that if you sum the lifespan of SOFT + MEDIUM = HARD, this allows a good variety of strategies on the races.
