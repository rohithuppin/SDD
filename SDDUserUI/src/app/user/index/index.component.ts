import { Component } from '@angular/core';
  
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { UserService } from '../user.service';
import { User } from '../user';
  
@Component({
  selector: 'app-index',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './index.component.html',
  styleUrl: './index.component.css'
})
export class IndexComponent {
  
  users: User[] = [];

  constructor(public userService: UserService) { }
      
  ngOnInit(): void {
    this.userService.getAll().subscribe((data: User[])=>{
      this.users = data;
      console.log(this.users);
    })  
  }
      
  deleteUser(id:number){
    this.userService.delete(id).subscribe(res => {
         this.users = this.users.filter(item => item.userId !== id);
         console.log('User deleted successfully!');
    })
  }  
}