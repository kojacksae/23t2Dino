/// <summary>
/// This script is just empty, this is holding these data types here.
/// In doing this these variables are accessible from everywhere they are considered global.
/// Keep in mind here it's not always a good idea to have everything global.
/// </summary>

//Enums
public enum InputActions {ActionOne, ActionTwo, ActionThree, ActionFour, ActionFive, ActionSix, ActionSeven, ActionEight, Move, Look, Start, Select}
public enum PlayerNumbers {PlayerOne = 1, PlayerTwo = 2, PlayerThree = 3, PlayerFour = 4}

// Events
public delegate void VoidEventNoPram();
public delegate void VoidEventOneParam<T1>(T1 paramOne);
public delegate void VoidEventTwoParam<T1, T2>(T1 paramOne, T2 paramTwo);
public delegate void VoidEventThreeParam<T1, T2, T3>(T1 paramOne, T2 paramTwo, T3 paramThree);