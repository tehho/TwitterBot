namespace TwitterBot.Domain
{
    public interface ITrainableFromText
    {
        void TrainFromText(TextContent text);
    }
}