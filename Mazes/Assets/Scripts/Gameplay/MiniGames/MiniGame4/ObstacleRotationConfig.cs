public class ObstacleRotationConfig {
    public ObstacleRotationConfig(bool direction, float speed) {
        Direction = direction;
        Duration = speed;
    }

    public bool Direction { get; private set; }
    public float Duration { get; private set; }
}
