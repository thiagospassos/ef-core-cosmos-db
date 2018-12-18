import { Component, Inject, Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class ApiService {
  constructor(
    private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string
  ) {}

  getForecasts() {
    return this.http.get<any[]>(
      this.baseUrl + "api/SampleData/WeatherForecasts"
    );
  }

  getPosts() {
    return this.http.get<any[]>(this.baseUrl + "api/Post/GetAll");
  }

  addPost(model: any) {
    return this.http.post<any>(this.baseUrl + "api/Post", model);
  }
}
