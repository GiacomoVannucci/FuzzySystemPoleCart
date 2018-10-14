
public class MembershipFunction{

    public float center;
    public float left;
    public float right;

    public float x;
    public float v;

    public MembershipFunction(float l, float c, float r) {

        center = c;
        left = l;
        right = r;
    }

    public float Triangular(float value) {

        if(value > left && value < center) {
            return 1 / (center - left) * (value - left);
        }

        if(value > center && value < right) {
            return -1 / (right - center) * (value - right);
        }

        if(value == center || (center == left && value <= left) || (center == right && value >= right)) {
            return 1f;
        }

        return 0f;
    }

}
