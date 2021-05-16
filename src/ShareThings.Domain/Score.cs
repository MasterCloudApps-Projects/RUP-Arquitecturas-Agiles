using ShareThings.Domain.Exceptions;

namespace ShareThings.Domain
{
    public class Score
    {
        const int MinimumScore = 1;
        const int MaximumScore = 5;

        public Score(int punctuation)
        {
            if (punctuation < MinimumScore || punctuation > MaximumScore)
                throw new ScoreOutRangeException($"The punctuation is not valid, the value has to be between {MinimumScore} and {MaximumScore}");

            this.Punctuation = punctuation;
        }

        public int Punctuation { get; private set; }
    }
}