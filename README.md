# Spinning Wheel
### Unity Version 2020.3.3f1

## Scripts
- Bet - Controls the UI system of increasing and decreasing bets 
- Coins - Controls the coins system to the UI
- Wheel2 - Manages the whole wheel system. The duration, spins, weights and prizes of the game. 
- Singleton - Helps in passing around references
- Audio/AudioManager - Selfexplanatory, manages the sounds.

## GameObjects and Inspector
- The GameManager Gameobject has the CoinManager and Bet script. The current coins and Bets can be adjusted on the inspector of this gameobject.
- Wheel Gameobject has the child of all 8 division of the wheel and it is called 1 to 8 respectively. It is using a text component and can be freely changed individually. 
In the case of needing to put an image on the wheel, changing the next component into an image component is necessary.
- Wheel Gameobject has access to the prizes and weights of each division.(I put the lower numbers with high weights and vice versa.). Other variables for the wheel can also be accessed from here
- Bet Parent Gameobject - has all the UI elements of the betting system including the increase and decrease button and the Current Bet number.
- Other UI elements would be the win text that changes every win after the spin,the coins text which shows the current coins you have and the play button which spins the wheel.
