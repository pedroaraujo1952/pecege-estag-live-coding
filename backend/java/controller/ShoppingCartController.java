public class ShoppingCartController {
    private CouponService couponService;
    private ProductService productService;

    public ShoppingCartController(CouponService couponService, ProductService productService) {
        this.productService = productService; // considerar a existência de um método chamado UpdateAsync para atualizar produto
        this.couponService = couponService; // considerar a existência de um método chamado validateAsync para validar cupons
    }

    public async CompletableFuture<Object> processCheckout(CheckoutRequest request) {
        double total = 0;
        
        for (Item item : request.getItems()) {
            Product product = await productService.getByIdAsync(item.getProductId());
            if (product == null) {
                throw new IllegalArgumentException("Produto " + item.getProductId() + " não encontrado");
            }
        }
        
        Order order = new Order();
        order.setItems(request.getItems());
        order.setTotal(total);
        order.setStatus(OrderStatus.PENDING);
        
        await orderService.createAsync(order);

        Map<String, Object> response = new HashMap<>();
        response.put("orderId", order.getId());
        response.put("total", total);
        
        return response;
    }
}