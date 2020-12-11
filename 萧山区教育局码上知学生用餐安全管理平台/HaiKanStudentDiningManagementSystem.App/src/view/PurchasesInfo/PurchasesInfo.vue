<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.demo.query.totalCount"
        :pageSize="stores.demo.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="23">
              <Col span="23" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <Button
                    icon="md-refresh"
                    title="刷新"
                    @click="handleRefresh"
                  ></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="添加"
                  >添加</Button
                >
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.demo.data"
          :columns="stores.demo.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
        >
          <template slot-scope="{ row, index }" slot="action">
            <Poptip
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
            >
              <Tooltip
                placement="top"
                content="删除"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  type="error"
                  v-can="'delete'"
                  size="small"
                  shape="circle"
                  icon="md-trash"
                ></Button>
              </Tooltip>
            </Poptip>
            <Tooltip
              placement="top"
              content="编辑"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip
              placement="top"
              content="详情"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleDetail(row)"
              ></Button>
            </Tooltip>
            <Tooltip
              placement="top"
              content="采购明细"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'infolist'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-contacts"
                @click="handleShowInfoList(row)"
              ></Button>
            </Tooltip>
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel.fields"
        ref="formdispatch"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="登记人员" prop="registerUserId">
            <Select
              filterable
              v-model="formModel.fields.registerUserId"
              style="width: 100%;"
              :disabled="checkShow()"
              :label-in-value="true"
              @on-change="Getreperson"
            >
              <Option
                v-for="item in usersList"
                :key="item.sysUserId"
                :value="item.sysUserId"
              >{{ item.name }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="登记日期" prop="registerDate">
            <Date-picker
              v-model="formModel.fields.registerDate"
              @on-change="formModel.fields.registerDate = $event"
              type="datetime"
              placeholder="请选择登记日期"
              style="width: 100%"
              :options="options3"
              :readonly="checkShow()"
            ></Date-picker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="采购人员" prop="purchaseUser">
           <Input
              v-model="formModel.fields.purchaseUser"
              :readonly="checkShow()"
              placeholder="请输入采购人员"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="采购日期" prop="purchaseDate">
            <Date-picker
              v-model="formModel.fields.purchaseDate"
              @on-change="formModel.fields.purchaseDate = $event"
              type="datetime"
              placeholder="请选采购日期"
              style="width: 100%"
              :options="options3"
              :readonly="checkShow()"
            ></Date-picker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="品种" prop="types">
            <Select
              filterable
              v-model="model1"
              multiple
              style="width: 100%;"
              :disabled="checkShow()"
              :label-in-value="true"
              @on-change="Gettypes"
            >
              <Option
                v-for="item in typesList"
                :key="item.typeId"
                :value="item.typeId"
              >{{ item.keyValue }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="供应商" prop="supplier">
            <Select
              filterable
              v-model="formModel.fields.supplier"
              style="width: 100%;"
              :disabled="checkShow()"
              :label-in-value="true"
              @on-change="Getsupplier1"
            >
              <Option
                v-for="item in supplierList"
                :key="item.partnerId"
                :value="item.fullName"
              >{{ item.fullName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="备注" prop="note">
            <Input
              type="textarea"
              v-model="formModel.fields.note"
              :readonly="checkShow()"
              placeholder="请输入备注"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon
                  type="ios-eye-outline"
                  style="float: left;"
                  @click.native="handleView(item.url)"
                ></Icon>
                <Icon
                  type="ios-trash-outline"
                  style="float: right;"
                  @click.native="handleRemove(item.name)"
                   v-show="!checkShow()"
                ></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
          <Divider dashed />
          <Upload
            ref="upload"
            :show-upload-list="false"
            :default-file-list="defaultList"
            :on-success="showUpResult"
            :on-progress="toUpResult"
            :format="['jpg','jpeg','png']"
            :max-size="5120"
            :data="{scene:'采购图片',groupType:'purchase'}"
            :on-format-error="handleFormatError"
            :on-exceeded-size="handleMaxSize"
            :before-upload="handleBeforeUpload"
            type="drag"
            :action="actionurl"
            :headers="postheaders"
            style="display: inline-block;width:58px;"
          >
            <div style="width: 58px;height:58px;line-height: 58px;">
              <Icon type="ios-camera" size="20"></Icon>
            </div>
          </Upload>
          <Modal title="查看图片" v-model="visible">
            <img :src="imgName" v-if="visible" style="width: 100%" />
          </Modal>
        </Row>
        <!-- <Row :gutter="16" v-if="checkShow()">
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon type="ios-eye-outline" @click.native="handleView(item.url)"></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
        </Row> -->
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitConsumable"
          v-if="!checkShow()"
          >保 存</Button
        >
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="formModel.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
    <Drawer
      title="采购明细列表"
      v-model="formInfo.opened"
      width="1200"
      :mask-closable="true"
      :mask="true"
    >
      <Card>
        <dz-table
          :totalCount="stores.mCheckInfo.query.totalCount"
          :pageSize="stores.mCheckInfo.query.pageSize"
          @on-page-change="handlePageChanged2"
          @on-page-size-change="handlePageSizeChanged2"
        >
          <div slot="searcher">
            <section class="dnc-toolbar-wrap">
              <Row :gutter="16">
                <Col span="16">
                  <Form inline @submit.native.prevent>
                    <FormItem>
                    </FormItem>
                  </Form>
                </Col>
                <Col span="8" class="dnc-toolbar-btns">
                  <ButtonGroup class="mr3">
                    <Button
                      v-can="'delete'"
                      class="txt-danger"
                      icon="md-trash"
                      title="删除"
                      @click="handleBatchCommand2('delete')"
                    ></Button>
                    <Button
                      icon="md-refresh"
                      title="刷新"
                      @click="handleRefresh2"
                    ></Button>
                  </ButtonGroup>
                  <Button
                    v-can="'create'"
                    icon="md-create"
                    type="primary"
                    @click="handleShowCreateWindow2"
                    title="添加"
                  >添加</Button>
                </Col>
              </Row>
            </section>
          </div>
          <Table
            slot="table"
            ref="tables"
            :border="false"
            size="small"
            :highlight-row="true"
            :data="stores.mCheckInfo.data"
            :columns="stores.mCheckInfo.columns"
            @on-select="handleSelect2"
            @on-selection-change="handleSelectionChange2"
            @on-refresh="handleRefresh2"
            :row-class-name="rowClsRender2"
            @on-page-change="handlePageChanged2"
            @on-page-size-change="handlePageSizeChanged2"
          >
            <!-- <template slot-scope="{row, index}" slot="auditState">
            <span>{{renderAuditState(row.auditState)}}</span>
            </template>-->
            <template slot-scope="{ row, index }" slot="action2">
              <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete2(row)">
                <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                  <Button
                    type="error"
                    v-can="'deletes'"
                    size="small"
                    shape="circle"
                    icon="md-trash"
                  ></Button>
                </Tooltip>
              </Poptip>
              <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit2(row)"
              ></Button>
              </Tooltip>
              <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
                <Button
                  v-can="'show'"
                  type="warning"
                  size="small"
                  shape="circle"
                  icon="md-search"
                  @click="handleDetail2(row)"
                ></Button>
              </Tooltip>
            </template>
          </Table>
        </dz-table>
      </Card>
    </Drawer>
    <Drawer
      :title="formTitle"
      v-model="formModel1.opened"
      width="400"
      :mask-closable="false"
      :mask="true"
    >
      <Form
        :model="formModel1.fields"
        ref="formdispatch2"
        :rules="formModel1.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <FormItem label="创建人" prop="createUserId">
            <Select
              filterable
              v-model="formModel1.fields.createUserId"
              style="width: 100%;"
              :disabled="checkShow2()"
              :label-in-value="true"
            >
              <Option
                v-for="item in usersList"
                :key="item.sysUserId"
                :value="item.sysUserId"
              >{{ item.name }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="货品名称" prop="foodName">
           <Input
              v-model="formModel1.fields.foodName"
              :readonly="checkShow2()"
              placeholder="请输入货品名称"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="单位名称" prop="unitName">
           <Input
              v-model="formModel1.fields.unitName"
              :readonly="checkShow2()"
              placeholder="请输入单位名称"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="规格名称" prop="modelName">
           <Input
              v-model="formModel1.fields.modelName"
              :readonly="checkShow2()"
              placeholder="请输入规格名称"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="采购数量" prop="number">
            <Input
              v-model="formModel1.fields.number"
              :readonly="checkShow2()"
              placeholder="请输入采购数量"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="供应商" prop="supplierId">
            <Select
              filterable
              v-model="formModel1.fields.supplierId"
              style="width: 100%;"
              :disabled="checkShow2()"
              :label-in-value="true"
              @on-change="Getsupplier"
            >
              <Option
                v-for="item in supplierList"
                :key="item.partnerId"
                :value="item.provideId"
              >{{ item.fullName }}</Option>
            </Select>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="生产日期" prop="productionDate">
            <Date-picker
              v-model="formModel1.fields.productionDate"
              @on-change="formModel1.fields.productionDate = $event"
              type="datetime"
              placeholder="请选择生产日期"
              style="width: 100%"
              :options="options3"
              :readonly="checkShow2()"
            ></Date-picker>
          </FormItem>
        </Row>
        <Row :gutter="16">
          <FormItem label="保质期" prop="expireDate">
            <Date-picker
              v-model="formModel1.fields.expireDate"
              @on-change="formModel1.fields.expireDate = $event"
              type="datetime"
              placeholder="请选择保质期"
              style="width: 100%"
              :options="options3"
              :readonly="checkShow2()"
            ></Date-picker>
          </FormItem>
        </Row>
      </Form>
      <div class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitConsumable2"
          v-if="!checkShow2()"
          >保 存</Button
        >
        <Button
          style="margin-left: 30px"
          icon="md-close"
          @click="formModel1.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
  </div>
</template>
<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  GetList, //显示列表
  GetCreate, //新增
  GetfoGet, //获取选定信息
  GetEdit, //编辑
  batchCommand, //批量删除
  deleteDepartment, //单个删除
  aGetList, //显示列表
  aGetCreate, //新增
  aGetShow, //获取选定信息
  abatchCommand, //批量删除
  adeleteDepartment, //单个删除
  aGetEdit, //编辑
  aGetPartList,
  aGetUsersList,
  GetTypesList
} from "@/api/PurchasesInfo/PurchasesInfo";
import config from "@/config";
import { getToken } from "@/libs/util";
export default {
  name: "PurchasesInfo",
  components: {
    DzTable,
  },
  data() {
    return {
      options3: {
        disabledDate(date) {
          return date && date.valueOf() > Date.now();
        },
      },
      commands: {
        delete: { name: "delete", title: "删除" },
      },

      uploadList: [],
      defaultList: [],
      actionurl: "",
      postheaders: "",
      imgName: "",
      loadingStatus: false,
      updisabled: false,
      visible: false,

      supplierList: [],
      typesList: [],
      usersList: [],
      model1: [],
      stores: {
        demo: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
          },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "id" },
            { title: "登记人员", key: "register", sortable: true, ellipsis: true },
            { title: "登记日期", key: "registerDate", sortable: true, ellipsis: true },
            { title: "采购人员", key: "purchaseUser", sortable: true, ellipsis: true },
            { title: "采购日期", key: "purchaseDate", sortable: true, ellipsis: true },
            { title: "品种", key: "type", sortable: true, ellipsis: true },
            {
              title: "操作",
              align: "center",
              width: 200,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
        mCheckInfo: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            isDelete: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
          },
          //列表显示
          columns: [
            { type: "selection", width: 50, key: "id" },
            { title: "货品名称", key: "foodName", sortable: true, ellipsis: true },
            { title: "单位名称", key: "unitName", sortable: true, ellipsis: true },
            { title: "规格名称", key: "modelName", sortable: true, ellipsis: true },
            { title: "采购数量", key: "number", sortable: true, ellipsis: true },
            { title: "添加日期", key: "createdAt", sortable: true, ellipsis: true },
            {
              title: "操作",
              align: "center",
              width: 200,
              className: "table-command-column",
              slot: "action2",
            },
          ],
          data: [],
        },
      },
      formModel: {
        opened: false,
        title: "创建申请",
        mode: "create",
        dFileName: "xxxx",
        selection: [],
        fields: {
          register: "",
          registerUserId: 0,
          registerDate: "",
          purchaseUser: "",
          purchaseDate: "",
          type: "",
          types:"",
          supplier: 0,
          ticketImgs: "",
          note: "",
        },
        rules: {
          register: [
            { type: "string", required: true, message: "登记人员不能为空" },
          ],
          registerDate: [
            {  required: true, message: "请选择登记日期" }
          ],
          purchaseUser: [
            { type: "string", required: true, message: "请输入采购人员" }
          ],
          purchaseDate: [
            {  required: true, message: "请选择采购日期" }
          ],
          type: [{ type: "string", required: true, message: "请输入品种" }]
        },
      },
      formModel1: {
        opened: false,
        title: "创建申请",
        mode: "create",
        dFileName: "xxxx",
        selection: [],
        fields: {
          foodName: "",
          unitName: "",
          modelName: "",
          number: "",
          supplier: "",
          supplierId: 0,
          createUserId:0,
          productionDate: "",
          expireDate: "",
          pid:0,
        },
        rules: {
          createUserId: [
            { required: true, message: "创建人员不能为空" },
          ]
        },
      },
      formInfo: {
        opened: false,
        selection: [],
        fields: {
        },
      },
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "新增信息";
      }
      if (this.formModel.mode === "edit") {
        return "编辑信息";
      }
      if (this.formModel.mode === "show") {
        return "信息详情";
      }
      return "";
    },
    formTitle1() {
      if (this.formModel1.mode === "create") {
        return "新增信息";
      }
      if (this.formModel1.mode === "edit") {
        return "编辑信息";
      }
      if (this.formModel1.mode === "show") {
        return "信息详情";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.id);
    }, //删除
    selectedRows2() {
      return this.formModel1.selection;
    },
    selectedRowsId2() {
      return this.formModel1.selection.map((x) => x.id);
    }, //删除
  },
  methods: {
    //页面加载
    loadDispatchList() {
      GetList(this.stores.demo.query).then((res) => {
        console.log(res);
        this.stores.demo.data = res.data.data;
        this.stores.demo.query.totalCount = res.data.totalCount;
      });
    },
    //页面加载1
    loadDispatchList2() {
      aGetList(this.stores.mCheckInfo.query).then((res) => {
        console.log(res);
        this.stores.mCheckInfo.data = res.data.data;
        this.stores.mCheckInfo.query.totalCount = res.data.totalCount;
      });
    },
    handleSelect(selection, row) {},
    handleSelect2(selection, row) {},
    //多选
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    //多选
    handleSelectionChange2(selection) {
      this.formModel1.selection = selection;
    },
    //翻页
    handlePageChanged(page) {
      this.stores.demo.query.currentPage = page;
      this.loadDispatchList();
    },
    //翻页
    handlePageChanged2(page) {
      this.stores.mCheckInfo.query.currentPage = page;
      this.loadDispatchList2();
    },
    //显示条数改变
    handlePageSizeChanged(pageSize) {
      this.stores.demo.query.pageSize = pageSize;
      this.loadDispatchList();
    },
    //显示条数改变
    handlePageSizeChanged2(pageSize) {
      this.stores.mCheckInfo.query.pageSize = pageSize;
      this.loadDispatchList2();
    },
    //行样式
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //行样式
    rowClsRender2(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    //搜索
    handleSearchDispatch() {
      this.loadDispatchList();
    },
    //搜索
    handleSearchDispatch2() {
      this.loadDispatchList2();
    },
    //刷新
    handleRefresh() {
      this.loadDispatchList();
    },
    //刷新
    handleRefresh2() {
      this.loadDispatchList2();
    },
    checkShow() {
      return this.formModel.mode === "show";
    },
    checkShow2() {
      return this.formModel1.mode === "show";
    },
    //单位
    doGetPartList() {
      aGetPartList().then((res) => {
        console.log(res);
        this.supplierList=res.data.data;
      });
    },
    Getsupplier(e){
      if (e != undefined) {
        this.formModel1.fields.supplier = e.label;
      }
    },
    Getsupplier1(e){
      if (e != undefined) {
        this.formModel.fields.supplier = e.label;
      }
    },
    //类别
    doGetTypesList() {
      GetTypesList().then((res) => {
        this.typesList = res.data.data;
      });
    },
    Gettypes(e){
     let data = [];
      for (let k = 0; k < e.length; k++) {
        data.push(e[k].label);
      }
      this.formModel.fields.type = data.join(",");
      this.formModel.fields.types = this.model1.join(",");
    },
    //人员
    doGetUsersList(){
        aGetUsersList().then(res=>{
            this.usersList = res.data.data;
        })
    },
    Getreperson(e){
        if (e != undefined) {
        this.formModel.fields.register = e.label;
      }
    },
    //详情显示
    handleDetail(e) {
      this.formModel.fields.ticketImgs="";
      this.defaultList=[];
      this.uploadList=[];
      this.formModel.mode = "show";
      this.formModel.opened = true;
      this.handleResetFormDispatch(); //清空表单
      this.doLoadData(e.id);
    },
    //详情显示
    handleDetail2(e) {
      this.formModel1.mode = "show";
      this.formModel1.opened = true;
      this.handleResetFormDispatch2(); //清空表单
      this.doLoadData2(e.id);
    },
    //添加按钮
    handleShowCreateWindow() {
      this.formModel.fields.ticketImgs="";
      this.defaultList=[];
      this.uploadList=[];
      this.formModel.mode = "create";
      this.handleResetFormDispatch(); //清空表单
      this.formModel.opened = true;
    },
    //添加按钮
    handleShowCreateWindow2() {
      this.formModel1.mode = "create";
      this.handleResetFormDispatch2(); //清空表单
      this.formModel1.opened = true;
    },
    //右边编辑
    handleEdit(row) {
      // if (this.formModel.fields.ticketImgs != null) {
      //   this.formModel.dFileName = "";
      //   this.$refs.upload.fileList = [
      //     { name: "", status: "finished", showProgress: false }
      //   ];
      // }
      this.formModel.mode = "edit";
      this.formModel.opened = true;
      this.formModel.fields.imgUrl="";
      this.defaultList=[];
      this.uploadList=[];
      this.handleResetFormDispatch(); //清空表单
      this.doLoadData(row.id);
    },
    //右边编辑
    handleEdit2(row) {
      this.formModel1.mode = "edit";
      this.formModel1.opened = true;
      this.handleResetFormDispatch2(); //清空表单
      this.doLoadData2(row.id);
    },
    //查询当前行信息
    doLoadData(id) {
      GetfoGet(id).then((res) => {
        console.log(res);
        if (res.data.code === 200) {
          this.model1 = [];
          this.formModel.fields = res.data.data;
          // if(res.data.data.registerUserId!=null){
          //   this.formModel.fields.registerUserId = res.data.data.registerUserId.toString();
          // }
          this.formModel.fields.purchaseDate = new Date(res.data.data.purchaseDate.replace(/-/g,"/"));
          this.formModel.fields.registerDate = new Date(res.data.data.registerDate.replace(/-/g,"/"));
          if (res.data.data.types != null) {
            if (res.data.data.types.split(",").length > 0) {
              this.model1=res.data.data.types.split(",");
            }
          }
          if (res.data.data.ticketImgs != null) {
            let list = res.data.data.ticketImgs.split(",");
            for (let i = 0; i < list.length; i++) {
              this.uploadList.push({
                url:
                  list[i],
                status: "finished",
                name: list[i],
                fileName: list[i]
              });
            }
          }
        }
      });
    },
    //查询当前行信息
    doLoadData2(id) {
      console.log(id)
      aGetShow(id).then((res) => {
        console.log(res);
        if (res.data.code === 200) {
          this.formModel1.fields = res.data.data;
        }
      });
    },
    validateUserForm() {
      let _valid = false;
      this.$refs["formdispatch"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    validateUserForm2() {
      let _valid = false;
      this.$refs["formdispatch2"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    //保存按钮
    handleSubmitConsumable() {
      let valid = this.validateUserForm();
      if (valid) {
        if (this.formModel.mode === "create") {
          this.docreateDispatch();
        }
        if (this.formModel.mode === "edit") {
          this.doEditDispatch();
        }
      }
    },
    //保存按钮
    handleSubmitConsumable2() {
      let valid = this.validateUserForm2();
      if (valid) {
        if (this.formModel1.mode === "create") {
          this.docreateDispatch2();
        }
        if (this.formModel1.mode === "edit") {
          this.doEditDispatch2();
        }
      }
    },
    //添加（保存）
    docreateDispatch() {
      GetCreate(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //添加（保存）
    docreateDispatch2() {
      aGetCreate(this.formModel1.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel1.opened = false; //关闭表单
          this.loadDispatchList2();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑（保存）
    doEditDispatch() {
      if (
        this.formModel.fields.registerDate != "" &&
        this.formModel.fields.registerDate != null
      ) {
        this.formModel.fields.registerDate = new Date(
          Date.parse(new Date(this.formModel.fields.registerDate)) +
            8 * 3600 * 1000
        );
      }
      if (
        this.formModel.fields.purchaseDate != "" &&
        this.formModel.fields.purchaseDate != null
      ) {
        this.formModel.fields.purchaseDate = new Date(
          Date.parse(new Date(this.formModel.fields.purchaseDate)) +
            8 * 3600 * 1000
        );
      }
      GetEdit(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.opened = false; //关闭表单
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //编辑（保存）
    doEditDispatch2() {
      if (
        this.formModel1.fields.productionDate != "" &&
        this.formModel1.fields.productionDate != null
      ) {
        this.formModel.fields.productionDate = new Date(
          Date.parse(new Date(this.formModel.fields.productionDate)) +
            8 * 3600 * 1000
        );
      }
      if (
        this.formModel.fields.expireDate != "" &&
        this.formModel.fields.expireDate != null
      ) {
        this.formModel.fields.expireDate = new Date(
          Date.parse(new Date(this.formModel.fields.expireDate)) +
            8 * 3600 * 1000
        );
      }
      aGetEdit(this.formModel1.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel1.opened = false; //关闭表单
          this.loadDispatchList2();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //清空
    handleResetFormDispatch() {
      this.$refs["formdispatch"].resetFields();
      this.model1=[];
    },
    //清空
    handleResetFormDispatch2() {
      this.$refs["formdispatch2"].resetFields();
    },
    //右边删除（单个删除）
    handleDelete(row) {
      this.doDelete(row.id);
    },
    //右边删除（单个删除）
    handleDelete2(row) {
      this.doDelete2(row.id);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteDepartment(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doDelete2(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      adeleteDepartment(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadDispatchList2();
          this.formModel1.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    //右上边删除按钮
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        },
      });
    },
    //右上边删除按钮
    handleBatchCommand2(command) {
      if (!this.selectedRowsId2 || this.selectedRowsId2.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand2(command);
        },
      });
    },
    //右上边删除
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel.selection = [];
          this.loadDispatchList();
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    //右上边删除
    doBatchCommand2(command) {
      abatchCommand({
        command: command,
        ids: this.selectedRowsId2.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.formModel1.selection = [];
          this.loadDispatchList2();
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleShowInfoList(row){
      console.log(row);
      this.stores.mCheckInfo.query.kw=row.id;
      this.formModel1.fields.pid=row.id;
      this.formInfo.opened = true;
      this.loadDispatchList2();
    },
    handleView(name) {
      this.imgName = name;
      this.visible = true;
      console.log(this.imgName);
    },
    handleRemove(file) {
      console.log(file);
          this.uploadList = this.uploadList.filter(x => x.name != file);
          this.formModel.fields.ticketImgs = this.uploadList
            .map(x => x.fileName)
            .join(",");
    },
    handleFormatError(file) {
      this.$Notice.warning({
        title: "The file format is incorrect",
        desc: "文件: " + file.name + " 不是png,jpg"
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "Exceeding file size limit",
        desc: "文件 " + file.name + " 太大,超过5M."
      });
    },
    handleBeforeUpload() {
      // const check = this.uploadList.length < 5;
      // if (!check) {
      //   this.$Notice.warning({
      //     title: "Up to five pictures can be uploaded."
      //   });
      // }
      // return check;
      return true;
    },
    toUpResult() {
      console.log(1111);
      console.log(this.$refs.upload.fileList);
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    async showUpResult(response, file, fileList) {
      console.log(this.$refs.upload.fileList);
      console.log(1);
      console.log(response);
      console.log(file);
      console.log(fileList);
      this.loadingStatus = false;
      this.updisabled = false;
      if (response.code == "200") {
        this.$Message.success(response.message);
        console.log(this.formModel.fields.ticketImgs);
        if (this.formModel.fields.ticketImgs != null) {
          if (this.formModel.fields.ticketImgs.length > 0) {
            this.formModel.fields.ticketImgs += ",";
          }
          this.formModel.fields.ticketImgs += response.data;
        } else {
          this.formModel.fields.ticketImgs = response.data;
        }
        await this.uploadList.push({
          url: response.data.replace("\\", "/"),
          status: "finished",
          name: response.data,
          fileName: response.data
        });
      } else {
        this.$Message.warning(response.message);
      }
    },
  },
  mounted() {
    this.loadDispatchList(); //页面加载
    this.doGetPartList();
    this.doGetTypesList();
    this.doGetUsersList();
  },
  created() {
    this.postheaders = {
      Authorization: "Bearer " + getToken(),
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/UpImage/ToUPPhoto/UpPhoto";
  },
};
</script>
<style>
.file1:hover + .fileimg1:hover {
  transform: scale(1.01, 1.01);
  box-shadow: 0 0 3px #1783ba;
}
.fileimg1 {
  width: 300px;
  height: 169px;
  float: left;
  z-index: 2;
}

.demo-upload-list {
  display: inline-block;
  width: 120px;
  height: 120px;
  text-align: center;
  line-height: 120px;
  border: 1px solid transparent;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
  position: relative;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
  margin-right: 4px;
}
.demo-upload-list img {
  width: 100%;
  height: 100%;
}
.demo-upload-list-cover {
  display: none;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
}
.demo-upload-list:hover .demo-upload-list-cover {
  display: block;
}
.demo-upload-list-cover i {
  color: #fff;
  font-size: 20px;
  cursor: pointer;
  margin: 0 2px;
}
</style>