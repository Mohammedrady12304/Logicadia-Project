import { Component,OnInit,ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router , RouterLink } from '@angular/router';
import { ParentService } from '../../core/services/parent.service';
import { ChildSummaryDto } from '../../core/models/parent.model';

@Component({
selector:'app-children',
imports:[CommonModule , RouterLink],
templateUrl:'./children.html',
styleUrl:'./children.css',
})

export class Children implements OnInit{

children:ChildSummaryDto[]=[];
isLoading=false;
errorMessage='';

constructor(
private parentService:ParentService,
private router:Router,
private cdr:ChangeDetectorRef
){}


ngOnInit(){
this.loadChildren();
}


loadChildren(){

this.isLoading=true;

this.parentService.getChildren().subscribe({

next:(data)=>{

this.children=data;
this.isLoading=false;
this.cdr.detectChanges();

},

error:()=>{

this.errorMessage='Failed to load children.';
this.isLoading=false;
this.cdr.detectChanges();

}

});

}



registerChild(){

this.router.navigate(['/parent/register-child']);

}



viewProgress(childId:number){

this.router.navigate(['/parent/child',childId,'progress']);

}



assignPath(childId:number){

this.router.navigate(['/parent/child',childId,'assign-path']);

}

}