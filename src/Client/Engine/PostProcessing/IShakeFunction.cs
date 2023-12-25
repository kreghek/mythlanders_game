namespace Client.Engine.PostProcessing;

public interface IShakeFunction
{
    ShakePower Calculate(double t);
}