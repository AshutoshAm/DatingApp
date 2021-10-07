import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  title = 'client';
  users:any

  constructor(private http:HttpClient){

  }
  ngOnInit(): void {

    this.http.get("https://localhost:44347/api/Users").subscribe(res=>{
      this.users=res;
      console.log(this.users);
    },error=>{
      console.log("this",error);
    })

  }

}
