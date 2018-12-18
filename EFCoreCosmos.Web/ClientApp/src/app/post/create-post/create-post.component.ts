import { Component, OnInit } from "@angular/core";
import { ApiService } from "../../core/services/api.service";
import { Router } from "@angular/router";

@Component({
  selector: "app-create-post",
  templateUrl: "./create-post.component.html",
  styleUrls: ["./create-post.component.css"]
})
export class CreatePostComponent implements OnInit {
  private post: any = {};

  constructor(private api: ApiService, private router: Router) {}

  ngOnInit() {}

  add() {
    this.api.addPost(this.post).subscribe(result => {
      console.log(result);
      this.router.navigate(["/"]);
    });
  }
}
