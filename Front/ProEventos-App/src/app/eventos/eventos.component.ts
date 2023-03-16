import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any ;
  constructor(private http: HttpClient) { }

  ngOnInit(): void { //metodo que é chamado antes de iniciar a aplicação
    this.getEventos();
  }

  public getEventos(): void{

    this.http.get('https://localhost:7255/api/eventos').subscribe(
      response => this.eventos = response,
      error => console.log(error)
    );
  }
}
