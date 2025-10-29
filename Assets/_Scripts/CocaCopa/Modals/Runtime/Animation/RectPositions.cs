using UnityEngine;

namespace CocaCopa.Modal.Runtime.Animation {
    internal class RectPositions {
        internal Vector2 visible;
        internal Vector2 hidden;
        internal RectPositions(Vector2 visible, Vector2 hidden) {
            this.visible = visible;
            this.hidden = hidden;
        }
    }
}
