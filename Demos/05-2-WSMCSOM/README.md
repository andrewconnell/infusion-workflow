Setup Console Application for SharePoint Online
===============================================
This console application is written to work with SharePoint Online in Office 365 in the sense that it uses an app only permission to login. Before running this sample you need to manually do a few things:

1. Upload the sandbox solution in the sample [05-1-SampleWorkflows](../05-1-SampleWorkflows)
1. Manually create an app
1. Assign it permissions & trust the app
1. Update the console app's `app.config` to use the new app credentials & update the `program.cs` to point the correct SharePoint site URL.

It's assumed you understand how to upload & activate a sandboxed solution so that isn't covered here. Just make sure you do this in the site you plan to test with.

You must also ensure you already have a Workflow History List & Workflow Task List created in the site that you will use to test this console application.

Manually Create a SharePoint App in SharePoint Online
-----------------------------------------------------
To create a new app manually in SharePoint, navigate to your sharePoint site. Once there, manually change the URL to **http://[site]/_layouts/15/AppRegNew.aspx**. Here you can create a new app. 

Click the two **Generate** buttons to create a new **App ID** (*aka: ClientID*) & **App Secret**. Make sure to copy these values down somewhere. Give the app a title & set the **App Domain** to **localhost**. Finally click **Save**.

Grant the App Permissions
-------------------------
Next up you need to give the app permissions... so navigate to **http://[site]/_layouts/15/AppInv.aspx**. Enter the **App ID** for your app and click the **Lookup** button to find the app. Once found, add the following to the **Permission Request XML** section and save your changes:

  ````xml
  <AppPermissionRequests AllowAppOnlyPolicy="true" >
    <AppPermissionRequest Scope="http://sharepoint/content/sitecollection/web" Right="FullControl" />
    <AppPermissionRequest Scope="http://sharepoint/content/sitecollection/web/list" Right="FullControl" />
  </AppPermissionRequests>
  ````

SharePoint will take you to the consent page prompting you to trust the app. 


Update Console App
------------------
Now update the console application. Open the `app.config` file and two app settings to the `<configuration>` element:

  ````xml
  <appSettings>
    <add key="ClientID" value="" />
    <add key="ClientSecret" value="" />
  </appSettings>
  ````

Set the values of these two attributes using the values you obtained when creating the app in SharePoint.

Next, open the `Program.cs` & update the `siteCollectionUrl` constant to be equal to the URL of the site collection you want to use.