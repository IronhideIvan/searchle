import { KeyboardKeys } from "./keyboardKeys";

export function convertToKeyboardKey(keyCode: string): KeyboardKeys | null {
  switch (keyCode) {
    case "KeyQ":
      return KeyboardKeys.Q;
    case "KeyW":
      return KeyboardKeys.W;
    case "KeyE":
      return KeyboardKeys.E;
    case "KeyR":
      return KeyboardKeys.R;
    case "KeyT":
      return KeyboardKeys.T;
    case "KeyY":
      return KeyboardKeys.Y;
    case "KeyU":
      return KeyboardKeys.U;
    case "KeyI":
      return KeyboardKeys.I;
    case "KeyO":
      return KeyboardKeys.O;
    case "KeyP":
      return KeyboardKeys.P;
    case "KeyA":
      return KeyboardKeys.A;
    case "KeyS":
      return KeyboardKeys.S;
    case "KeyD":
      return KeyboardKeys.D;
    case "KeyF":
      return KeyboardKeys.F;
    case "KeyG":
      return KeyboardKeys.G;
    case "KeyH":
      return KeyboardKeys.H;
    case "KeyJ":
      return KeyboardKeys.J;
    case "KeyK":
      return KeyboardKeys.K;
    case "KeyL":
      return KeyboardKeys.L;
    case "KeyZ":
      return KeyboardKeys.Z;
    case "KeyX":
      return KeyboardKeys.X;
    case "KeyC":
      return KeyboardKeys.C;
    case "KeyV":
      return KeyboardKeys.V;
    case "KeyB":
      return KeyboardKeys.B;
    case "KeyN":
      return KeyboardKeys.N;
    case "KeyM":
      return KeyboardKeys.M;
    case "Backspace":
      return KeyboardKeys.Delete;
    case "Enter":
      return KeyboardKeys.Enter;
    default:
      return null;
  }
}