import { useEffect } from 'react';

/**
 * A custom React hook for handling keyboard key presses.
 * 
 * @param targetKey - The key to listen for (e.g., 'Escape', 'Enter')
 * @param handler - The callback function to execute when the key is pressed
 * @param modifiers - Optional modifier keys that must be pressed with the target key (ctrl, alt, shift, meta)
 */
export const useKeyPress = (
  targetKey: string,
  handler: (event: KeyboardEvent) => void,
  modifiers: { ctrl?: boolean; alt?: boolean; shift?: boolean; meta?: boolean } = {}
): void => {
  useEffect(() => {
    // The function that handles the keydown event
    const downHandler = (event: KeyboardEvent) => {
      // Check if all required modifiers are pressed
      const ctrlPressed = modifiers.ctrl ? event.ctrlKey : true;
      const altPressed = modifiers.alt ? event.altKey : true;
      const shiftPressed = modifiers.shift ? event.shiftKey : true;
      const metaPressed = modifiers.meta ? event.metaKey : true;
      
      // If the pressed key matches the target key and all required modifiers are pressed
      if (
        event.key === targetKey && 
        ctrlPressed && 
        altPressed && 
        shiftPressed &&
        metaPressed
      ) {
        handler(event);
      }
    };

    // Add event listener when the component using this hook mounts
    window.addEventListener('keydown', downHandler);
    
    // Return a cleanup function to remove the event listener
    // when the component using this hook unmounts
    return () => {
      window.removeEventListener('keydown', downHandler);
    };
  }, [targetKey, handler, modifiers]); // Re-run effect if these dependencies change
};
