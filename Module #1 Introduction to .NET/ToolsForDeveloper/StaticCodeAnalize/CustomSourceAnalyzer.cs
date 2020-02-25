using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StyleCop;
using StyleCop.CSharp;

namespace StaticCodeAnalize
{
    [SourceAnalyzer(typeof(CsParser))]
    public class CustomSourceAnalyzer : SourceAnalyzer
    {
        public override void AnalyzeDocument(CodeDocument document)
        {
            var csharpDocument = document as CsDocument;
            if (csharpDocument != null)
            {
                csharpDocument.WalkDocument(
                    new CodeWalkerElementVisitor<CustomSourceAnalyzer>(this.VisitElement),
                    new CodeWalkerStatementVisitor<CustomSourceAnalyzer>(this.VisitStatement),
                    new CodeWalkerExpressionVisitor<CustomSourceAnalyzer>(this.VisitExpression),
                    this);
            }
        }


        private bool VisitElement(CsElement element, CsElement parentElement, CustomSourceAnalyzer context)
        {
            if (element.ElementType != ElementType.Class)
            {
                return true;
            }

            if (((Class)element).BaseClass != "System.Web.Controller")
            {
                return true;
            }

            if (element.Name.EndsWith("Controller"))
            {
                return true;
            }

            context.AddViolation(element, "ClassInheritedByControllerShouldBeEndedByController", element.Declaration.Name);
            return false;
        }

        private bool VisitStatement(Statement statement, Expression parentExpression, Statement parentStatement, CsElement parentElement, CustomSourceAnalyzer context)
        {
            // Add your code here.
            return true;
        }

        private bool VisitExpression(Expression expression, Expression parentExpression, Statement parentStatement, CsElement parentElement, CustomSourceAnalyzer context)
        {
            // Add your code here.
            return true;
        }
    }
}
