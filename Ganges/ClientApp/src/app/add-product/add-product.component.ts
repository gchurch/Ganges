import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Product } from '../product'
import { ProductService } from '../product.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

  productForm:  FormGroup;

  // Injecting the ProductService dependency. The property for the parameter is created for us.
  constructor(private productService: ProductService) { }

  ngOnInit() {
    // https://angular.io/guide/reactive-forms
    // https://angular.io/guide/form-validation
    // https://angular.io/api/forms/Validators
    this.productForm = new FormGroup({
      title: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      seller: new FormControl('', Validators.required),
      price: new FormControl(0, [Validators.required, Validators.min(0.01)]),
      quantity: new FormControl(0, [Validators.required, Validators.min(1)]),
      imageUrl: new FormControl('', Validators.required),
    });
  }

  onSubmit() {
    // Create a product object from the values in the form. The product id has to be zero when you add a product
    // to the database.
    var product: Product = {
      id: 0,
      title: this.productForm.value.title,
      description: this.productForm.value.description,
      seller: this.productForm.value.seller,
      price: parseInt(this.productForm.value.price),
      quantity: parseInt(this.productForm.value.quantity),
      imageUrl: this.productForm.value.imageUrl
    }
    // Print the product.
    console.log(product);
    // Send the product to the server.
    this.productService.addProduct(product).subscribe(
      output => {
        console.log(output);
        console.log("The product has been added to the database.");
      }
    );
    // Reset the form
    this.productForm.reset();
  }

}
