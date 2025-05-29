class ShoppingCartController {
    private couponService: ICouponService;
    private productService: IProductService;
    private orderService: IOrderService;

    constructor(couponService: ICouponService, productService: IProductService, orderService: IOrderService) {
        this.productService = productService; // considerar a existência de um método chamado UpdateAsync para atualizar produto
        this.couponService = couponService; // considerar a existência de um método chamado validateAsync para validar cupons
        this.orderService = orderService;
    }

    async processCheckout(request: CheckoutRequest): Promise<CheckoutResponse> {
        let total = 0;
        
        for (const item of request.items) {
            const product = await this.productService.getByIdAsync(item.productId);
            if (product == null) {
                throw new Error(`Produto ${item.productId} não encontrado`);
            }
        }
        
        const order: Order = {
            items: request.items,
            total: total,
            status: 'Pending'
        };
        
        await this.orderService.createAsync(order);

        return { orderId: order.id, total: total };
    }
}