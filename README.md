# HappyCrud
C# Standard Class Library that perform CRUD on IList &lt;T> while tracking Edit State and Validations 

[![nuget](https://user-images.githubusercontent.com/55330747/137058843-2cd885d8-223a-4954-8c13-67bfcb41ff99.png)](https://www.nuget.org/packages/Arora.HappyCRUD/)

# Functions
here is all the Functions and their useage in HappyCRUD
   ```csharp
using HappyCRUD; //Using HappyCRUD 
List<Education> list = new(); //Create List of T
list.Create(); //Create new item in array and enables Edit State
list.Cancel(); //Shut down the edit state and restore previous state 
list.Delete(); //Shut down the edit state and delete the item
list.Save(); //shut down the edit state and save the item
list.MoveUp(1); //Takes item via index and move it up
list.MoveDown(0); //Takes item via index and move it down
list.StartEdit(1); //Stat edit state for item via index
list.IsModelValid(); // checks if all items in list is valid
list.First().InEditState = default; //Checks if item is in edit state 
CRUD.IsInEditState = default; //Checks if there is any item is in edit state in the whole model. 
```
# Blazor Example 
To Create item.
```razor
  <button type="button" class="btn btn-primary"  @onclick="()=>CVModel.CV.Educations.Create()">Add Education</button>
```
To Cancel, Save or Delete
```razor
@if (CVModel.CV.Educations[j].InEditState)
  {                   
      <button type="button" class="btn btn-outline-dark" @onclick="()=>CVModel.CV.Educations.Cancel()">Cancel</button>
      <button type="button" class="btn btn-outline-success" @onclick="()=>CVModel.CV.Educations.Save()">Save</button>
      <button type="button" class="btn btn-outline-danger" @onclick="()=>CVModel.CV.Educations.Delete()">Delete</button>
  }
```
To edit  or move item up and down
```razor
@if (!CRUD.IsInEditState)
  {
     @if(!CVModel.CV.Educations[j].InEditState)
       {
          <button type="button" class="btn btn-primary" @onclick="()=>CVModel.CV.Educations.MoveUp(j)">&uarr;</button>
          <button type="button" class="btn btn-primary" @onclick="()=>CVModel.CV.Educations.MoveDown(j)">&darr; </button>
          <button type="button" class="btn btn-primary"  @onclick="()=>CVModel.CV.Educations.StartEdit(j)">Edit </button>
       }
  }   
 ```
 bind to edit value
 ```razor
 @if( CVModel.CV.Educations[j].InEditState )
   {
       <div class="form-group">
         <label>School Name</label>
          <InputText class="form-control" id="LocationName"  @bind-Value="CVModel.CV.Educations![j].LocationName"/>
          <ValidationMessage For="@(()=> CVModel.CV.Educations![j].LocationName)" />
       </div> 
       <div class="form-group">
          <label>Year</label>
          <InputText class="form-control" id="Year" @bind-Value="CVModel.CV.Educations![j].Year"/>
          <ValidationMessage For="@(()=> CVModel.CV.Educations![j].Year)" />
      </div>
   }
else
   {
      <div class="form-group">
          <label>School Name</label>
          <label class="form-control" id="LocationName">@CVModel.CV.Educations![j].LocationName</label>
          <ValidationMessage For="@(()=> CVModel.CV.Educations![j].LocationName)" />
     </div> 
     <div class="form-group">
          <label>Year</label>
          <label class="form-control" id="LocationName">@CVModel.CV.Educations![j].Year</label>
           <ValidationMessage For="@(()=> CVModel.CV.Educations![j].Year)" />
      </div>    
   }
 ```
# Model Sample
 IValidation Interfaces
```C#
using HappyCRUD;
     public class Education : IValidation
    {
        public virtual string? LocationName { get; set; }
        public string? Year { get; set; }
        public bool IsValid() => !string.IsNullOrWhiteSpace(LocationName) && !string.IsNullOrWhiteSpace(Year);
        public bool InEditState { get; set; }
    }
```
# Model Sample Extra
  ![HappyCRUD](https://user-images.githubusercontent.com/55330747/156047501-9231debe-7b5c-4fdf-bf1d-1060f731434c.jpg)


