using UnityEngine;
using System.Collections;

namespace Pathfinding {
	/** Sets the destination of an AI to the position of a specified object.
	 * This component should be attached to a GameObject together with a movement script such as AIPath, RichAI or AILerp.
	 * This component will then make the AI move towards the #target set on this component.
	 *
	 * \see #Pathfinding.IAstarAI.destination
	 *
	 * \shadowimage{aidestinationsetter.png}
	 */
	[UniqueComponent(tag = "ai.destination")]
	[HelpURL("http://arongranberg.com/astar/docs/class_pathfinding_1_1_a_i_destination_setter.php")]
	public class AIDestinationSetter : VersionedMonoBehaviour {
		public Transform patrolPath;
		private int currPathIndex = 0;
		private bool distracted = false;
		private Vector3 gotoDistraction;
		private DeviceController dc;
		/** The object that the AI should move to */
		public Transform target;
		IAstarAI ai;

		void OnEnable () {
			ai = GetComponent<IAstarAI>();
			// Update the destination right before searching for a path as well.
			// This is enough in theory, but this script will also update the destination every
			// frame as the destination is used for debugging and may be used for other things by other
			// scripts as well. So it makes sense that it is up to date every frame.
			if (ai != null) ai.onSearchPath += Update;
		}

		void OnDisable () {
			if (ai != null) ai.onSearchPath -= Update;
		}

		public void noticedDistraction(Vector3 point, DeviceController deviceController) {
			Debug.Log("noticed Distraction");
			distracted = true;
			gotoDistraction = point;
			dc = deviceController;
		}

		/** Updates the AI's destination every frame */
		void Update () {
			if (ai != null) {
				//ai.destination = target.position;
				if (distracted) {
					ai.destination = gotoDistraction;
					// If we found the distraction, turn it off and go back to the path
					if (Vector2.Distance(transform.position, gotoDistraction) < 0.4f) {
						dc.Deactivate();
						distracted = false;
						Debug.Log("No longer distracted");
					}
				} else {
				// Travel along the patrol path

					var patrolDest = patrolPath.GetChild(currPathIndex).transform.position;
					// If we are close to the current patrol point
					if (Vector2.Distance(transform.position, patrolDest) < 0.4f) {
						// Then move on to the next one
						currPathIndex = (currPathIndex + 1) % patrolPath.childCount;
					}
					ai.destination = patrolPath.GetChild(currPathIndex).transform.position;
				}
			}
		}
	}
}
