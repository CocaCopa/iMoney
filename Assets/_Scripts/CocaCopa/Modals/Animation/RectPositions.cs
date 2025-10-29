using UnityEngine;

namespace CocaCopa.Modal.Animation {
    internal class RectPositions {
        public Vector2 visible;
        public Vector2 hidden;
        public RectPositions(Vector2 visible, Vector2 hidden) {
            this.visible = visible;
            this.hidden = hidden;
        }
    }
}
