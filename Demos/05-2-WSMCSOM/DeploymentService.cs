using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.WorkflowServices;

namespace WSMCSOM {
  public class DeploymentService {
    private const string _validWorkflow = "<Activity x:Class=\"SharePointProject1.Vacation_Request.Workflow\" xmlns=\"http://schemas.microsoft.com/netfx/2009/xaml/activities\" mc:Ignorable=\"sap2010\" xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\" sap2010:ExpressionActivityEditor.ExpressionActivityEditor=\"C#\" xmlns:sap2010=\"http://schemas.microsoft.com/netfx/2010/xaml/activities/presentation\" xmlns:sco=\"clr-namespace:System.Collections.ObjectModel;assembly=mscorlib\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"><TextExpression.NamespacesForImplementation><sco:Collection x:TypeArguments=\"x:String\"><x:String>System</x:String><x:String>System.Collections.Generic</x:String><x:String>System.Data</x:String><x:String>System.Text</x:String></sco:Collection></TextExpression.NamespacesForImplementation><TextExpression.ReferencesForImplementation></TextExpression.ReferencesForImplementation><Sequence></Sequence></Activity>";
    private const string _invalidWorkflow = "<Activity x:Class=\"SharePointProject1.Vacation_Request.Workflow\" xmlns=\"http://schemas.microsoft.com/netfx/2009/xaml/activities\" mc:Ignorable=\"sap2010\" xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\" sap2010:ExpressionActivityEditor.ExpressionActivityEditor=\"C#\" xmlns:sap2010=\"http://schemas.microsoft.com/netfx/2010/xaml/activities/presentation\" xmlns:sco=\"clr-namespace:System.Collections.ObjectModel;assembly=mscorlib\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"><TextExpression.NamespacesForImplementation><sco:Collection x:TypeArguments=\"x:String\"><x:String>System</x:String><x:String>System.Collections.Generic</x:String><x:String>System.Data</x:String><x:String>System.Text</x:String></sco:Collection></TextExpression.NamespacesForImplementation><TextExpression.ReferencesForImplementation><sco:Collection x:TypeArguments=\"AssemblyReference\"><AssemblyReference>mscorlib</AssemblyReference><AssemblyReference>System</AssemblyReference><AssemblyReference>System.Core</AssemblyReference><AssemblyReference>System.Data</AssemblyReference><AssemblyReference>System.ServiceModel</AssemblyReference><AssemblyReference>System.Xml</AssemblyReference><AssemblyReference>System.Xml.Linq</AssemblyReference></sco:Collection></TextExpression.ReferencesForImplementation><Sequence></Sequence></Activity>";

    /// <summary>
    /// Write out all installed workflows.
    /// </summary>
    public static void ShowAllInstalledWorkflows(ref ClientContext clientConext,
                                                 ref WorkflowServicesManager wfServicesManager) {
      // connect to deployment service
      WorkflowDeploymentService depService = wfServicesManager.GetWorkflowDeploymentService();

      // get all installed workflows
      bool showOnlyPublishedWorkflows = true;
      WorkflowDefinitionCollection wfDefintions = depService.EnumerateDefinitions(showOnlyPublishedWorkflows);
      clientConext.Load(wfDefintions);
      clientConext.ExecuteQuery();

      // write all
      Console.WriteLine();
      Console.WriteLine("All Installed Workflows:");
      foreach (WorkflowDefinition wfDefintion in wfDefintions) {
        Console.WriteLine("{0} - {1}",
          wfDefintion.Id,
          wfDefintion.DisplayName);
      }
    }

    /// <summary>
    /// Validate a good workflow.
    /// </summary>
    public static void ValidateGoodWorkflow(ref ClientContext clientConext,
                                            ref WorkflowServicesManager wfServicesManager) {
      // connect to deployment service
      WorkflowDeploymentService depService = wfServicesManager.GetWorkflowDeploymentService();

      Console.WriteLine();
      Console.WriteLine("Validating workflow:");
      Console.WriteLine(_validWorkflow);

      ClientResult<string> result = depService.ValidateActivity(_validWorkflow);
      clientConext.ExecuteQuery();

      Console.WriteLine();
      Console.Write("Validation result: ");
      if (string.IsNullOrEmpty(result.Value))
        Console.WriteLine("workflow validated");
      else
        Console.WriteLine("error: " + result.Value);
    }

    /// <summary>
    /// Validate a bad workflow.
    /// </summary>
    public static void ValidateBadWorkflow(ref ClientContext clientConext,
                                           ref WorkflowServicesManager wfServicesManager) {
      // connect to deployment service
      WorkflowDeploymentService depService = wfServicesManager.GetWorkflowDeploymentService();

      Console.WriteLine();
      Console.WriteLine("Validating workflow:");
      Console.WriteLine(_invalidWorkflow);

      ClientResult<string> result = depService.ValidateActivity(_invalidWorkflow);
      clientConext.ExecuteQuery();

      Console.WriteLine();
      Console.Write("Validation result: ");
      if (string.IsNullOrEmpty(result.Value))
        Console.WriteLine("workflow validated");
      else
        Console.WriteLine("error: " + result.Value);
    }

    /// <summary>
    /// Install a new workflow definition.
    /// </summary>
    public static void InstallWorkflow(ref ClientContext clientConext,
                                       ref WorkflowServicesManager wfServicesManager,
                                       Guid listId) {
      // connect to deployment service
      WorkflowDeploymentService depService = wfServicesManager.GetWorkflowDeploymentService();

      string workflowStamp = DateTime.Now.ToString();

      WorkflowDefinition workflowDefinition = new WorkflowDefinition(clientConext);
      workflowDefinition.Xaml = _validWorkflow;
      workflowDefinition.DisplayName = "Custom-" + workflowStamp;
      workflowDefinition.Description = "new custom workflow created " + workflowStamp;
      workflowDefinition.RestrictToType = "List"; // ["List" | "Site" | ""]
      workflowDefinition.RestrictToScope = listId.ToString();

      Console.WriteLine();
      Console.WriteLine("Creating new workflow");
      Console.WriteLine("   saving workflow...");
      ClientResult<Guid> result = depService.SaveDefinition(workflowDefinition);
      clientConext.ExecuteQuery();

      Console.WriteLine("   publishing workflow...");
      depService.PublishDefinition(result.Value);
      clientConext.ExecuteQuery();

      Console.WriteLine("Workflow published... use SharePoint Designer 2013 to see it.");
    }


    /// <summary>
    /// Get one installed workflow definition.
    /// </summary>
    public static Guid GetOneInstalledWorkflow(ref ClientContext clientConext,
                                               ref WorkflowServicesManager wfServicesManager) {
      // connect to deployment service
      WorkflowDeploymentService depService = wfServicesManager.GetWorkflowDeploymentService();

      // get all installed workflows
      bool showOnlyPublishedWorkflows = true;
      WorkflowDefinitionCollection wfDefintions = depService.EnumerateDefinitions(showOnlyPublishedWorkflows);
      clientConext.Load(wfDefintions);
      clientConext.ExecuteQuery();

      return wfDefintions.First().Id;
    }

  }
}
