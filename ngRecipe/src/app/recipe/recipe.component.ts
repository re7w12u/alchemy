import { Component, OnInit } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/Rx';


declare function unescape(s: string): string; 
declare function escape(s: string): string; 

@Component({
    selector: 'app-recipe',
    templateUrl: './recipe.component.html',
    styleUrls: ['./recipe.component.css']
})
export class RecipeComponent implements OnInit {


    ingredients = [];
    RecipeType: Object;
    RecipeLevel: Object;
    CookingTime: Object;
    PreparationTime: Object;
    Quantity: Object;
    Diet: Object;
    Budget: Object;
    recipe: string;
    params: string = "&apikey=xxx&outputMode=json&extract=entities,keywords&sentiment=1&maxRetrieve=1&url=https://www.ibm.com/us-en/";
    endpoint = "https://gateway-a.watsonplatform.net/calls/text/TextGetCombinedData";

    ready: boolean = false;
    failed: boolean = false;
    errorMsg: string;

    img = { normal: "./Content/ibm-watson.png", search: "./Content/giphy.gif" };
    src: string = this.img.normal;


    constructor(private http: Http) { }

    ngOnInit() {
    }

    Fetch(): void {
        this.src = this.img.search;
        setTimeout(() => this.src = this.img.normal, 5500);

        let text = this.recipe;
        //text = this.CleanUp(text);
        text = this.encode_utf8(text);

        this.Clear();
        this.FetchMLAnnotator(text);
        this.FetchRuleAnnotator(text);
    };

    //CleanUp(text: string) {
    //    //const pattern: string = ;
    //    const r: RegExp = /\`|\~|\!|\@|\#|\$|\%|\^|\&|\*|\(|\)|\+|\=|\[|\{|\]|\}|\||\\|\'|\<|\,|\.|\>|\?|\/|\""|\;|\:/
    //    return text.replace(r, '');
    //}

    Clear(): void{
        this.RecipeType = null;
        this.RecipeLevel = null;
        this.Diet = null;
        this.Budget = null;
        this.PreparationTime = null;
        this.CookingTime = null;
        this.Quantity = null;
        this.ingredients = [];
        this.ready = true;
    }

    FetchMLAnnotator(text: string) {

        const model = "&model=xxx";
        const body = "text=" + text + model + this.params;

        const headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded');

        this.http.post(this.endpoint, body, { headers: headers })
            .map(res => res.json())
            .subscribe(
            (response) => {
                console.log(response);
                if (response.status === "ERROR") {
                    this.failed = true;
                    this.errorMsg = response.statusInfo;
                } if (response.status === "OK") {
                    this.RecipeType = response.entities.find(x => x.type === "RECIPE_TYPE");
                    this.RecipeLevel = response.entities.find(x => x.type === "RECIPE_LEVEL");
                    this.Diet = response.entities.find(x => x.type === "RECIPE_DIET");
                    this.Budget = response.entities.find(x => x.type === "RECIPE_BUDGET");

                    for (let item of response.entities) {
                        if (item.type === "INGREDIENTS") {
                            this.ingredients.push(item.text);
                        }
                    }
                }
            },
            () => this.failed = true
            );
    }

    FetchRuleAnnotator(text: string) {
        const model = "&model=xxx";
        const body = "text=" + text + model + this.params;

        const headers = new Headers();
        headers.append('Content-Type', 'application/x-www-form-urlencoded');

        this.http.post(this.endpoint, body, { headers: headers })
            .map(res => res.json())
            .subscribe(
            (response) => {
                console.log(response);
                if (response.status === "ERROR") {
                    this.failed = true;
                    this.errorMsg = response.statusInfo;
                } if (response.status === "OK") {
                    console.log(response.entities);
                    this.PreparationTime = response.entities.find(x => x.type === "QUANTITY");
                    this.CookingTime = response.entities.find(x => x.type === "COOKING_TIME");
                    this.Quantity = response.entities.find(x => x.type === "PREPARATION_TIME");
                }
            },
            () => this.failed = true
            );


    }

    encode_utf8(s) {
        return unescape(encodeURIComponent(s));
    }

    decode_utf8(s) {
        return decodeURIComponent(escape(s));
    }
}

