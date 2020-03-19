using System;

namespace BCLLibrory
{
    public class Rule
    {
        public string Expression { get; }
        public string Target { get; }
        public OutputNameConfiguration OutputNameConfiguration { get; }

        public Rule(string expression, string target, OutputNameConfiguration outputNameConfiguration)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            OutputNameConfiguration = outputNameConfiguration;
        }
    }
}
