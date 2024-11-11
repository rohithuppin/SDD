import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
  
import { UserService } from '../user.service';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../user';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
  
@Component({
  selector: 'app-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './edit.component.html',
  styleUrl: './edit.component.css'
})
export class EditComponent {
  
  userId!: number;
  user!: User;
  form!: FormGroup;
      
  /*------------------------------------------
  --------------------------------------------
  Created constructor
  --------------------------------------------
  --------------------------------------------*/
  constructor(
    public userService: UserService,
    private route: ActivatedRoute,
    private router: Router
  ) { }
      
  /**
   * Write code on Method
   *
   * @return response()
   */
  ngOnInit(): void {
    this.userId = this.route.snapshot.params['userId'];
    this.userService.find(this.userId).subscribe((data: User)=>{
      this.user = data;
    }); 
        
    this.form = new FormGroup({
      title: new FormControl('', [Validators.required]),
      body: new FormControl('', Validators.required)
    });
  }
      
  /**
   * Write code on Method
   *
   * @return response()
   */
  get f(){
    return this.form.controls;
  }
      
  /**
   * Write code on Method
   *
   * @return response()
   */
  submit(){
    console.log(this.form.value);
    this.userService.update(this.userId, this.form.value).subscribe((res:any) => {
         console.log('User updated successfully!');
         this.router.navigateByUrl('user/index');
    })
  }
  
}