using System;

namespace BCLLibrory
{
    public class Rule
    {
        string Expression { get; }
        string Target { get; }
        OutputNameConfiguration OutputNameConfiguration { get; }

        public Rule(string expression, string target, OutputNameConfiguration outputNameConfiguration)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            Target = target ?? throw new ArgumentNullException(nameof(target));
            OutputNameConfiguration = outputNameConfiguration;
        }
    }
}
