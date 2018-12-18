import { Component, OnInit } from "@angular/core";
import { ApiService } from "../core/services/api.service";

@Component({
  selector: "app-post",
  templateUrl: "./post.component.html",
  styleUrls: ["./post.component.css"]
})
export class PostComponent implements OnInit {
  posts: any[] = [];

  constructor(private api: ApiService) {}

  ngOnInit() {
    this.api.getPosts().subscribe(result => (this.posts = result));
  }
}
