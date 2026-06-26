import { Component,OnInit,ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute,Router } from '@angular/router';
import { ScenarioService } from '../../core/services/scenario.service';
import { ScenarioPlayDto,ChoiceOptionDto,SubmitResultDto } from '../../core/models/scenario.model';

@Component({
selector:'app-scenario-play',
imports:[CommonModule],
templateUrl:'./scenario-play.html',
styleUrl:'./scenario-play.css',
})
export class ScenarioPlay implements OnInit{

scenario:ScenarioPlayDto|null=null;
selectedChoiceId:number|null=null;
result:SubmitResultDto|null=null;

isLoading=false;
isSubmitting=false;
errorMessage='';

constructor(
private scenarioService:ScenarioService,
private route:ActivatedRoute,
private router:Router,
private cdr:ChangeDetectorRef
){}

ngOnInit(){

const id=Number(this.route.snapshot.paramMap.get('id'));
this.loadScenario(id);

}

loadScenario(id:number){

this.isLoading=true;

this.scenarioService.getScenarioById(id).subscribe({

next:(data)=>{

this.scenario=data;
this.isLoading=false;
this.cdr.detectChanges();

},

error:()=>{

this.errorMessage='Failed to load scenario.';
this.isLoading=false;
this.cdr.detectChanges();

}

});

}

selectChoice(choice:ChoiceOptionDto){

if(this.result)return;

this.selectedChoiceId=choice.id;

}

submitChoice(){

if(!this.scenario||!this.selectedChoiceId)return;

this.isSubmitting=true;

this.scenarioService.submitChoice(this.scenario.id,{
scenarioId:this.scenario.id,
chosenChoiceId:this.selectedChoiceId
}).subscribe({

next:(res)=>{

this.result=res;
this.isSubmitting=false;

},

error:()=>{

this.errorMessage='Failed to submit answer.';
this.isSubmitting=false;

}

});

}

continueNext(){

const nextId=this.result?.nextScenarioId;

this.result=null;
this.selectedChoiceId=null;

if(nextId){

this.router.navigate(['/scenarios',nextId]);
this.loadScenario(nextId);

}else{

this.router.navigate(['/levels']);

}

}

}